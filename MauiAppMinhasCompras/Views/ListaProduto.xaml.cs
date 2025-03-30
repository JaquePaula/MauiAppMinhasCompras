using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
	ObservableCollection<Produto> lista = new ObservableCollection<Produto>();

	public ListaProduto()
	{
		InitializeComponent();

		lst_produtos.ItemsSource = lista;
	}

	protected async override void OnAppearing()
	{
		try
		{
			lista.Clear();

			List<Produto> tmp = await App.Db.GetAll();

			tmp.ForEach(i => lista.Add(i));
		}
		catch (Exception ex)
		{
			await DisplayAlert("Ops", ex.Message, "OK");
		}
	}

	private void ToolbarItem_Clicked_add(object sender, EventArgs e)
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

	private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
	{
		try
		{
			string q = e.NewTextValue;

			lista.Clear();

			List<Produto> tmp = await App.Db.Search(q);

			tmp.ForEach(i => lista.Add(i));
		}
		catch (Exception ex)
		{
			await DisplayAlert("Ops", ex.Message, "OK");
		}
	}

	private void ToolbarItem_Clicked_soma(object sender, EventArgs e)
	{
		double soma = lista.Sum(i => i.Total);

		string msg = $"O total é {soma:C}";

		DisplayAlert("Total dos Produtos", msg, "OK");
	}

	private async void MenuItem_Clicked(object sender, EventArgs e)
	{
		try
		{
			MenuItem selecinado = sender as MenuItem;

			Produto p = selecinado.BindingContext as Produto;

			bool confirm = await DisplayAlert(
					"Tem Certeza?", $"Remover {p.Descricao}?", "Sim", "Não");

			if (confirm)
			{
				await App.Db.Delete(p.Id);
				lista.Remove(p);
			}
		}
		catch (Exception ex)
		{
			await DisplayAlert("Ops", ex.Message, "OK");
		}
	}

	private void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
	{
		try
		{
			Produto p = e.SelectedItem as Produto;

			Navigation.PushAsync(new Views.EditarProduto
			{
				BindingContext = p,
			});
		}
		catch (Exception ex)
		{
			DisplayAlert("Ops", ex.Message, "OK");
		}
	}

	private async void picker_categoria_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{

			string categoriaSelecionada = picker_categoria.SelectedItem.ToString();


			List<Produto> tmp = await App.Db.GetAll();


			if (categoriaSelecionada != "Todos")
			{
				tmp = tmp.Where(p => p.Categoria == categoriaSelecionada).ToList();
			}


			lista.Clear();
			tmp.ForEach(i => lista.Add(i));

			lst_produtos.ItemsSource = lista;
		}
		catch (Exception ex)
		{
			await DisplayAlert("Ops", ex.Message, "OK");
		}
	}

	private void ToolbarItem_Clicked_relatorio(object sender, EventArgs e)
	{
		var relatorio = lista
				.GroupBy(i => i.Categoria)
				.Select(g => new { Categoria = g.Key, Total = g.Sum(i => i.Quantidade * i.Preco) })
				.ToList();

		if (relatorio.Count == 0)
		{
			DisplayAlert("Relatório", "Nenhum dado disponível.", "OK");
			return;
		}

		string msg = "Relatório de Vendas por Categoria:\n\n";
		foreach (var item in relatorio)
		{
			msg += $"{item.Categoria}: {item.Total:C}\n";
		}

		DisplayAlert("Relatório", msg, "OK");
	}


}



