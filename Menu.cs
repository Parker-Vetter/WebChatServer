using System;
using System.Threading.Tasks;
using Spectre.Console;

class Menu
{
    private readonly Server _server;

    public Menu(Server server)
    {
        _server = server;
    }

    public void Run()
    {
        AnsiConsole.Markup("[underline red]Hello[/] World!");
        // Create a table
        var table = new Table();
        
        // Add some columns
        table.AddColumn("Foo");
        table.AddColumn(new TableColumn("Bar").Centered());
        
        // Add some rows
        table.AddRow("Baz", "[green]Qux[/]");
        table.AddRow(new Markup("[blue]Corgi[/]"), new Panel("Waldo"));
        
        // Render the table to the console
        AnsiConsole.Write(table);
    }
}
