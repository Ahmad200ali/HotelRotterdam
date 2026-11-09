using System.Globalization;
using System.Text;

// always use Dutch settings, so prices are shown in euros on every computer
CultureInfo.CurrentCulture = new CultureInfo("nl-NL");

// needed to show the euro sign in the console
Console.OutputEncoding = Encoding.UTF8;

Menu.Start();
