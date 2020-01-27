using System;
using System.Drawing;
using System.Windows.Forms;

namespace OrganizadorChurrasco
{
   public partial class Form1 : Form
   {
      //=========================== CRIA O FORMULARIO ============================================
      public Form1()
      {
         InitializeComponent();
      }
      //=========================== MÉTODOS ======================================================
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

            //Adiciona o item no datagridview.
            dgvItens.Rows.Add(produto.Descricao, produto.UnidadeMedida, produto.Quantidade, produto.Preco.ToString("C"), (produto.Quantidade * produto.Preco).ToString("C"), produto.Status);

            //Atualiza o valor total.                
            txtTotal.Text = (Convert.ToDouble(txtTotal.Text.Substring(3)) + (produto.Quantidade * produto.Preco)).ToString("C");

            //Atualiza a quantidade de itens pendentes.                             
            lblPendentes.Text = (Convert.ToInt32(lblPendentes.Text) + 1).ToString();

            dgvItens.CurrentRow.Cells["status"].Style.ForeColor = Color.Red;

            LimparCampos();
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
