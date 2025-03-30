using System.Threading.Tasks;
using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class NovoProduto : ContentPage
{
	public NovoProduto()
	{
		InitializeComponent();
	}

	private async void ToolbarItem_Clicked_add(object sender, EventArgs e)
	{
		try
		{
			Produto p = new Produto
			{
				Descricao = txt_descricao.Text,
				Quantidade = Convert.ToDouble(txt_quantidade.Text),
				Preco = Convert.ToDouble(txt_preco.Text),
				Categoria = picker_categoria.SelectedItem?.ToString()
			};

			await App.Db.Insert(p);
			await DisplayAlert("Sucesso", "Registro inserido", "OK");
			await Navigation.PopAsync();
		}
		catch (Exception ex)
		{
			await DisplayAlert("Ops", ex.Message, "OK");
		}
	}
}

