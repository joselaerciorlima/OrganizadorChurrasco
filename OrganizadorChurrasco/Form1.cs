using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;

namespace OrganizadorChurrasco
{
   public partial class Form1 : Form
   {
      //=========================== STRING DE CONEXAO ============================================
      SqlConnection conexao = new SqlConnection(@"Data Source=.\SQLEXPRESS; Initial Catalog=BDCHURRASCO;User ID=sa;Password=sa");
      //=========================== CRIA O FORMULARIO ============================================
      public Form1()
      {
         InitializeComponent();
         Carregar();
      }
      //=========================== MÉTODOS ======================================================
      void Carregar()
      {
         try
         {
            conexao.Open();
            string query = "SELECT * FROM TBITENS ORDER BY descricao";
            SqlCommand command = new SqlCommand(query, conexao);
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            da.Fill(dt);

            dgvItens.DataSource = dt;

            if (dgvItens.SelectedRows.Count > 0)
            {
               dgvItens.CurrentRow.Selected = false;
            }

            if (dt.Rows.Count > 0)
            {
               SqlCommand commandTotal = new SqlCommand("SELECT SUM(total) FROM TBITENS", conexao);

               double retorno = Convert.ToDouble(commandTotal.ExecuteScalar());
               txtTotal.Text = retorno.ToString("C");

               SqlCommand commandPendentes = new SqlCommand("SELECT COUNT(codigo) FROM TBITENS WHERE status = 'Pendente'", conexao);
               lblPendentes.Text = commandPendentes.ExecuteScalar().ToString();
            }

            SqlCommand commandInfo = new SqlCommand("SELECT TOP 1 * FROM TBINFO", conexao);
            SqlDataAdapter daInfo = new SqlDataAdapter(commandInfo);
            DataTable dtInfo = new DataTable();
            daInfo.Fill(dtInfo);

            foreach (DataRow item in dtInfo.Rows)
            {
               mtcData.SelectionStart = Convert.ToDateTime(item["data"]);
               dtpHora.Value = Convert.ToDateTime(item["hora"]);
            }

         }
         catch (Exception erro)
         {
            MessageBox.Show("Não foi possível selecionar os dados. Detalhes: " + erro.Message);
         }
         finally
         {
            conexao.Close();
         }
      }
      //------------------------------------------------------------------------------------------
      void LimparCampos()
      {
         txtDescricao.Clear();
         txtPreco.Clear();
         cbxMedida.SelectedIndex = -1;
         nudQtde.Value = 1;
         txtDescricao.Focus();

         if (dgvItens.SelectedRows.Count > 0)
            dgvItens.CurrentRow.Selected = false;
      }
      //------------------------------------------------------------------------------------------
      void MudarStatus()
      {
         string status;

         if (dgvItens.CurrentRow.Cells["status"].Value.ToString() == "Pendente")
            status = "Comprado";
         else
            status = "Pendente";

         string mensagem = "Mudar o status do produto " + dgvItens.CurrentRow.Cells["item"].Value.ToString().ToUpper() + " para " + status.ToUpper() + "?";

         DialogResult escolha = MessageBox.Show(mensagem, "STATUS DO ITEM", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
         if (escolha == DialogResult.Yes)
         {
            dgvItens.CurrentRow.Cells["status"].Value = status;
            if (status == "Comprado")
            {
               lblPendentes.Text = (Convert.ToInt32(lblPendentes.Text) - 1).ToString();
               dgvItens.CurrentRow.Cells["status"].Style.ForeColor = Color.Green;
            }
            else
            {
               lblPendentes.Text = (Convert.ToInt32(lblPendentes.Text) + 1).ToString();
               dgvItens.CurrentRow.Cells["status"].Style.ForeColor = Color.Red;
            }
            LimparCampos();

            SqlCommand command = new SqlCommand("UPDATE TBITENS SET status = '" + status + "' WHERE codigo = " + dgvItens.CurrentRow.Cells["codigo"].Value.ToString(), conexao);

            conexao.Open();
            command.ExecuteNonQuery();
            conexao.Close();
         }
      }
      //------------------------------------------------------------------------------------------
      void Adicionar()
      {
         if (string.IsNullOrWhiteSpace(txtDescricao.Text))
         {
            MessageBox.Show("Você precisa informar o NOME do Item!", "Op's!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            txtDescricao.Focus();
         }
         else if (cbxMedida.SelectedIndex < 0)
         {
            MessageBox.Show("Você precisa selecionar uma UNIDADE DE MEDIDA!", "Op's!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            cbxMedida.Focus();
         }
         else if (!double.TryParse(txtPreco.Text, out double preco))
         {
            MessageBox.Show("Você precisa informar um PREÇO VÁLIDO!", "Op's!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            txtPreco.Clear();
            txtPreco.Focus();
         }
         else
         {
            Produto produto = new Produto();
            produto.Descricao = txtDescricao.Text.Trim();
            produto.UnidadeMedida = cbxMedida.SelectedItem.ToString();
            produto.Quantidade = (int)nudQtde.Value;
            produto.Preco = Convert.ToDouble(txtPreco.Text);
            produto.Status = "Pendente";

            //Atualiza o valor total.                
            txtTotal.Text = (Convert.ToDouble(txtTotal.Text.Substring(3)) + (produto.Quantidade * produto.Preco)).ToString("C");

            //Atualiza a quantidade de itens pendentes.                             
            lblPendentes.Text = (Convert.ToInt32(lblPendentes.Text) + 1).ToString();

            dgvItens.CurrentRow.Cells["status"].Style.ForeColor = Color.Red;

            LimparCampos();

            //Enviar para o banco de dados o item cadastrado.
            string query = "INSERT INTO TBITENS (descricao, unidadeMedida, quantidade, preco, total, status) VALUES (@descricao, @unidademedida,@quantidade,@preco,@total,@status); SELECT @@IDENTITY";

            SqlCommand command = new SqlCommand(query, conexao);
            command.Parameters.AddWithValue("@descricao", produto.Descricao);
            command.Parameters.AddWithValue("@unidademedida", produto.UnidadeMedida);
            command.Parameters.AddWithValue("@quantidade", produto.Quantidade);
            command.Parameters.AddWithValue("@preco", produto.Preco);
            command.Parameters.AddWithValue("@total", produto.Preco * produto.Quantidade);
            command.Parameters.AddWithValue("@status", produto.Status);
            command.CommandType = CommandType.Text;

            try
            {
               conexao.Open();
               int codigo = Convert.ToInt32(command.ExecuteScalar());
               if (codigo > 0)
               {
                  conexao.Close();
                  Carregar();


                  MessageBox.Show("Item Cadastrado!");
               }
            }
            catch (Exception erro)
            {
               MessageBox.Show("Não foi possível cadastrar o item. Detalhes: " + erro.Message); ;
            }
            finally
            {
               conexao.Close();
            }
         }
      }
      //------------------------------------------------------------------------------------------
      void Remover()
      {
         if (dgvItens.SelectedRows.Count == 0)
         {
            MessageBox.Show("Você precisa Selecionar um item para remover!", "Op's!", MessageBoxButtons.OK, MessageBoxIcon.Information);
         }
         else
         {
            if (MessageBox.Show("Deseja remover o item " + dgvItens.CurrentRow.Cells["item"].Value.ToString().ToUpper() + " da lista?", "Remover", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
               txtTotal.Text = (Convert.ToDouble(txtTotal.Text.Substring(3)) - Convert.ToDouble(dgvItens.CurrentRow.Cells["total"].Value.ToString().Substring(3))).ToString("C");

               lblPendentes.Text = (Convert.ToInt32(lblPendentes.Text) - 1).ToString();

               dgvItens.Rows.Remove(dgvItens.CurrentRow);

               LimparCampos();

               //Remover no banco de dados.
               try
               {
                  conexao.Open();
                  string query = "DELETE FROM TBITENS WHERE codigo = " + dgvItens.CurrentRow.Cells["codigo"].Value.ToString();
                  SqlCommand command = new SqlCommand(query, conexao);

                  conexao.Open();

                  if (command.ExecuteNonQuery() > 0)
                  {
                     MessageBox.Show("Item Removido!");
                  }
               }
               catch (Exception erro)
               {
                  MessageBox.Show(erro.Message);
               }
               finally
               {
                  conexao.Close();
               }
            }
         }
      }
      //=========================== CONTROLES ====================================================
      private void btnAdicionar_Click(object sender, EventArgs e)
      {
         Adicionar();
      }
      //------------------------------------------------------------------------------------------
      private void btnRemover_Click(object sender, EventArgs e)
      {
         Remover();
      }
      //------------------------------------------------------------------------------------------
      private void mtcData_DateChanged(object sender, DateRangeEventArgs e)
      {
         if (mtcData.SelectionStart < DateTime.Today)
         {
            MessageBox.Show("Você não pode selecionar uma data anterior a data de hoje!", "Op's!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            mtcData.SelectionStart = DateTime.Today;
         }
         else
            lblDiasFaltam.Text = "Faltam " + Convert.ToInt32((mtcData.SelectionStart - DateTime.Today).TotalDays).ToString() + " dias";

         try
         {
            if (conexao.State == ConnectionState.Closed)
               conexao.Open();

            string query = "IF(NOT EXISTS(SELECT * FROM TBINFO WHERE data = '" + mtcData.SelectionStart + "')) BEGIN DELETE TBINFO WHERE data <> '" + mtcData.SelectionStart + "'; INSERT INTO TBINFO (data, hora) VALUES (@data, @hora) END";

            SqlCommand command = new SqlCommand(query, conexao);
            command.Parameters.AddWithValue("@data", mtcData.SelectionStart);
            command.Parameters.AddWithValue("@hora", dtpHora.Value.ToShortTimeString());

            command.CommandType = CommandType.Text;

            command.ExecuteNonQuery();
         }
         catch (Exception erro)
         {
            MessageBox.Show("Algo deu errado! Detalhes: " + erro.Message);
         }
         finally
         {
            conexao.Close();
         }
      }
      //------------------------------------------------------------------------------------------
      private void dgvItens_DoubleClick(object sender, EventArgs e)
      {
         MudarStatus();
      }
      //------------------------------------------------------------------------------------------
      private void dgvItens_KeyDown(object sender, KeyEventArgs e)
      {
         if (e.KeyCode == Keys.Enter)
            MudarStatus();
         else if (e.KeyCode == Keys.F12)
            Remover();
      }
      //------------------------------------------------------------------------------------------
      private void Form1_KeyDown(object sender, KeyEventArgs e)
      {
         if (e.KeyCode == Keys.F11)
         {
            Adicionar();
         }
      }
   }
}
