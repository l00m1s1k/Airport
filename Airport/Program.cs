using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

enum FlightStatus
{
    OnTime,
    Delayed,
    Cancelled,
    Boarding,
    InFlight
}

class Flight
{
    public string FlightNumber { get; set; }
    public string Airline { get; set; }
    public string Destination { get; set; }
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
    public FlightStatus Status { get; set; }
    public TimeSpan Duration { get; set; }
    public string AircraftType { get; set; }
    public string Terminal { get; set; }
}

class FlightInformationSystem
{
    private List<Flight> flights;

    public FlightInformationSystem()
    {
        flights = new List<Flight>();
    }

    public void LoadFlightsFromJson(string filePath)
    {
        try
        {
            string jsonContent = File.ReadAllText(filePath);
            var flightsData = JsonConvert.DeserializeObject<FlightData>(jsonContent);

            if (flightsData != null && flightsData.Flights != null)
            {
                flights.AddRange(flightsData.Flights);
            }
            else
            {
                Console.WriteLine("Invalid JSON format.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading JSON file: {ex.Message}");
        }
    }

    public void DisplayFlights()
    {
        foreach (var flight in flights)
        {
            Console.WriteLine($"Flight Number: {flight.FlightNumber}");
            Console.WriteLine($"Airline: {flight.Airline}");
            Console.WriteLine($"Destination: {flight.Destination}");
            Console.WriteLine($"Departure Time: {flight.DepartureTime}");
            Console.WriteLine($"Arrival Time: {flight.ArrivalTime}");
            Console.WriteLine($"Status: {flight.Status}");
            Console.WriteLine($"Duration: {flight.Duration}");
            Console.WriteLine($"Aircraft Type: {flight.AircraftType}");
            Console.WriteLine($"Terminal: {flight.Terminal}");
            Console.WriteLine(new string('-', 30));
        }
    }

    public List<Flight> GetFlightsByAirline(string airline)
    {
        return flights.FindAll(f => f.Airline.Equals(airline, StringComparison.OrdinalIgnoreCase))
            .OrderBy(f => f.DepartureTime)
            .ToList();
    }

    public List<Flight> GetDelayedFlights()
    {
        return flights.FindAll(f => f.Status == FlightStatus.Delayed)
            .OrderBy(f => f.DepartureTime)
            .ToList();
    }

    // Додайте інші методи для обробки запитів, які ви описали
}

class FlightData
{
    public List<Flight> Flights { get; set; }
}

class Program
{
    static void Main()
    {
        var flightSystem = new FlightInformationSystem();
        flightSystem.LoadFlightsFromJson("flights.json");

        Console.WriteLine("All Flights:");
        flightSystem.DisplayFlights();

        Console.WriteLine("\nFlights by Airline (WizAir):");
        var wizAirFlights = flightSystem.GetFlightsByAirline("WizAir");
        wizAirFlights.ForEach(f => Console.WriteLine(f.FlightNumber));

        Console.WriteLine("\nDelayed Flights:");
        var delayedFlights = flightSystem.GetDelayedFlights();
        delayedFlights.ForEach(f => Console.WriteLine(f.FlightNumber));
    }
}
