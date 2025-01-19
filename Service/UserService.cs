using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using milestone2.Models;

public class UserService
{
    private static readonly string DesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
    private static readonly string FolderPath = Path.Combine(DesktopPath, "LocalDB");
    private static readonly string FilePath = Path.Combine(FolderPath, "appdata.json");
    private string SelectedCurrency = "NPR";

    public AppData LoadData()
    {
        if (!File.Exists(FilePath)) return new AppData();
        try
        {
            var json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<AppData>(json) ?? new AppData();
        }
        catch
        {
            return new AppData();
        }
    }

    public void SaveData(AppData data)
    {
        EnsureFolderExists();
        var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FilePath, json);
    }

    public List<User> LoadUsers()
    {
        var appData = LoadData();
        return appData.Users;
    }

    public void SaveUsers(List<User> users)
    {
        var appData = LoadData();
        appData.Users = users;
        SaveData(appData);
    }

    public string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

    public bool ValidatePassword(string inputPassword, string storedPassword)
    {
        var hashedInputPassword = HashPassword(inputPassword);
        return hashedInputPassword == storedPassword;
    }

    private void EnsureFolderExists()
    {
        if (!Directory.Exists(FolderPath)) Directory.CreateDirectory(FolderPath);
    }

    public decimal CalculateBalance(int userId, AppData data)
    {
        var userTransactions = data.Transactions.Where(t => t.UserId == userId).ToList();
        decimal totalCredit = userTransactions.Sum(t => t.Credit);
        decimal totalDebit = userTransactions.Sum(t => t.Debit);
        return totalCredit - totalDebit;
    }

    public string GetCurrency() => SelectedCurrency;

    public void SetCurrency(string currency) => SelectedCurrency = currency;

    public decimal ConvertAmount(decimal amount, string targetCurrency)
    {
        var conversionRates = new Dictionary<string, decimal>
        {
            { "USD", 1m },
            { "EUR", 0.9m },
            { "NPR", 120m }
        };

        if (conversionRates.ContainsKey(SelectedCurrency) && conversionRates.ContainsKey(targetCurrency))
        {
            decimal rate = conversionRates[targetCurrency] / conversionRates[SelectedCurrency];
            return Math.Round(amount * rate, 2);
        }

        return amount;
    }
}
