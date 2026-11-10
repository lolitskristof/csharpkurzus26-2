using System.Text.Json;

using E5XVC8.Models;
using E5XVC8.Interfaces;

namespace E5XVC8.Repositories;

public class JsonCarRepository : ICarRepository
{
    private readonly string _filePath;

    public JsonCarRepository(string filePath)
    {
        _filePath = filePath;
    }

    public CarRentalData Load()
    {
        try
        {
            // Első indításkor még nem létezik mentett adatfájl.
            if (!File.Exists(_filePath)) return new CarRentalData();

            string json = File.ReadAllText(_filePath);

            return JsonSerializer.Deserialize<CarRentalData>(json) ?? throw new InvalidDataException("A JSON-fájl nem tartalmaz érvényes adatokat.");
        }
        catch (JsonException ex)
        {
            throw new InvalidDataException("Hibás JSON-fájl: " + ex.Message, ex);
        }
    }

    public void Save(CarRentalData data)
    {
        try
        {
            // Létrehozzuk az adatmappát, ha még nem létezik.
            string? directory = Path.GetDirectoryName(_filePath);

            if (!string.IsNullOrEmpty(directory)) Directory.CreateDirectory(directory);

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string json = JsonSerializer.Serialize(data, options);

            File.WriteAllText(_filePath, json);
        }
        catch (IOException ex)
        {
            throw new InvalidOperationException("Hiba történt a fájl mentésekor.", ex);
        }
        catch (UnauthorizedAccessException ex)
        {
            throw new InvalidOperationException("Nincs jogosultság a fájl mentéséhez.", ex);
        }
    }
}