namespace Hephaistos_Maui;
using Hephaistos_Maui.View;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
	}

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RegistrationPage());
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        string username = UsernameEntry.Text;
        string password = PasswordEntry.Text;
        bool stayLoggedIn = StayLoggedInCheckBox.IsChecked;

        
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            await DisplayAlert("Hiba", "Felhaszn�l�n�v �s jelsz� megad�sa k�telez�!", "OK");
            return;
        }

        
        bool success = await MauiProgram.LoginAsync(username, password, stayLoggedIn);

        if (success)
        {
            
            await DisplayAlert("Siker", "Bejelentkez�s sikeres!", "OK");
            
            await Navigation.PushAsync(new MainPage());
        }
        else
        {
            
            await DisplayAlert("Hiba", "Hib�s bejelentkez�si adatok!", "OK");
        }
    }
}