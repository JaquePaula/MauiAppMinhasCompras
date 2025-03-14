using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
	ObservableCollection<Produto> lista = new ObservableCollection<Produto>();
	List<Produto> listaOriginal = new List<Produto>(); // agenda 4

	public ListaProduto()
	{
		InitializeComponent();

		lst_produtos.ItemsSource = lista;
	}

	protected async override void OnAppearing()
	{
		// List<Produto> tmp = await App.Db.GetAll();
		// tmp.ForEach(i => lista.Add(i));

		try
		{
			listaOriginal = await App.Db.GetAll(); 
			AtualizarLista(""); 
		}
		catch (Exception ex)
		{
			await DisplayAlert("Ops", ex.Message, "OK");
		}
	}

	// private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
	// {
	// 	string q = e.NewTextValue;
	// 	lista.Clear();
	// 	List<Produto> tmp = await App.Db.Search(q);
	// 	tmp.ForEach(i => lista.Add(i));
	// }

	private void txt_search_TextChanged(object sender, TextChangedEventArgs e) // agenda 4
	{
		AtualizarLista(e.NewTextValue); 
	}

	private void AtualizarLista(string filtro) // agenda 4
	{
		lista.Clear();
		var produtosFiltrados = listaOriginal
				.Where(p => p.Descricao.Contains(filtro, StringComparison.OrdinalIgnoreCase))
				.ToList();

		produtosFiltrados.ForEach(p => lista.Add(p));
	}

	private void ToolbarItem_Clicked(object sender, EventArgs e)
	{
		try
		{
			Navigation.PushAsync(new Views.NovoProduto());

		}
		catch (Exception ex)
		{
			DisplayAlert("Ops", ex.Message, "OK");
		}

	}

	private void ToolbarItem_Clicked_1(object sender, EventArgs e)
	{
		double soma = lista.Sum(i => i.Total);

		string msg = $"O total é {soma:C}";

		DisplayAlert("Total dos Produtos", msg, "OK");
	}

	private void MenuItem_Clicked(object sender, EventArgs e)
	{

	}

}