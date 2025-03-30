using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class EditarProduto : ContentPage
{
	public EditarProduto()
	{
		InitializeComponent();
	}

	private async void ToolbarItem_Clicked_editar(object sender, EventArgs e)
	{
		try
		{
			Produto? produto_anexado = BindingContext as Produto;

			string categoriaSelecionada = picker_categoria.SelectedItem?.ToString();
			await DisplayAlert("Categoria Selecionada", categoriaSelecionada, "OK");

			Produto p = new Produto
			{
				Id = produto_anexado.Id,
				Descricao = txt_descricao.Text,
				Quantidade = Convert.ToDouble(txt_quantidade.Text),
				Preco = Convert.ToDouble(txt_preco.Text),
				Categoria = categoriaSelecionada
			};

			await App.Db.Update(p);

			await DisplayAlert("Sucesso!", "Registro Atualizado", "OK");
			await Navigation.PopAsync();
		}
		catch (Exception ex)
		{
			await DisplayAlert("Ops", ex.Message, "OK");
		}
	}
}