using System.Drawing;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

// Array of category names
string[] categories = new string[] { "Category", "Product", "Price" };

// String builders to accumulate user input and price to temporary store data to List
StringBuilder categ = new StringBuilder();// = new string[];
StringBuilder prod = new StringBuilder();// = new string[];
int price = 0;

// Display positions
int dispPosX = 0;
int dispPosY = 2;

// Positions for displaying the product list
int prodListPosX = 0;
int prodListPosY = 2;

// Counter for displaying products
int counter = 0;

// Flags and variables for input handling
int inputFlag = 0;
int milliseconds = 1100;
int prodId = 1;

// Function to capitalize the first character of a string
string FirstCharToUpper(string input)
{
    return Regex.Replace(input, "^[a-z]", c => c.Value.ToUpper());
}

// Instances of the Display class for different displays
Display display = new Display(ConsoleColor.DarkYellow, ConsoleColor.White, ConsoleColor.Red, ConsoleColor.Yellow, ConsoleColor.Green, dispPosX, dispPosY);
Display prodList = new Display(ConsoleColor.DarkYellow, ConsoleColor.Cyan, ConsoleColor.Yellow, prodListPosX, prodListPosY);

// List to store products
List<Product> productList = new List<Product>();

// Display initial message
display.Print("To enter a new product - Follow the steps | to Quit enter [Q/q] [Quit] [Exit]", 0, 0 ,0);

while (true)
{   // Prompt user for store input based on the current input flag
    display.Print($"Enter a {categories[inputFlag]}: ", 1);
    string input = Console.ReadLine();

    // Check for exit command
    if (input.ToUpper() == "Q" || input.ToUpper() == "QUIT" || input.ToUpper() == "EXIT")
    {
        break;
    }

    // Input handling based on the input flag
    // Handling category input
    if (inputFlag == 0)
    {   // Checking if input is Null or Empty
        if (string.IsNullOrEmpty(input))
        {
            display.Print("String is Empty! Nice Try!, Try again!", 2, 0, 3);
            Thread.Sleep(milliseconds);
        }
        else
        {
            categ.Append(input);
            inputFlag = 1;
        }
    }// Handling product input
    else if (inputFlag == 1)
    {   // Checking if input is Null or Empty
        if (string.IsNullOrEmpty(input))
        {
            display.Print("String is Empty! Nice Try! Try again!", 2, 0, 3);
            Thread.Sleep(milliseconds);
        }
        else
        {
            prod.Append(input);
            inputFlag = 2;
        }
    }// Handling price input
    else if (inputFlag == 2)
    {   // Checking if input can convert to integer price 
        if (Int32.TryParse(input, out price))
        {   // Adding collected Category, Product and Price till List object
            // and clear categ and prod then printout "ProductList is updated"
            productList.Add(new Product(prodId++, FirstCharToUpper(categ.ToString()), FirstCharToUpper(prod.ToString()), price));
            categ.Clear();
            prod.Clear();
            inputFlag = 0;
            display.Print("ProductList is updated!", 4, 0, 3);
            Thread.Sleep(milliseconds);
/*
            foreach (Product product in listProduct)
            {
                prodList.Print(product, 1, prodListPosX, prodListPosY + counter);
                counter++;
            }
            Thread.Sleep(milliseconds);
*/
        }
        else
        {
            display.Print("Price input is not a number, Try again!", 2, 0, 3);
            Thread.Sleep(milliseconds);
        }
    }
 }

// Sorting the product list
List<Product> sortedProdList = productList.OrderBy(Product => Product.ProductPrice).ToList();

// Displaying the product list
foreach (Product product in sortedProdList)
{
    prodList.Print(product, 1, prodListPosX, prodListPosY);
    prodListPosY++;
}
// Calculate the sum of price with LINQ 
prodList.ClearAltCursurPos();
prodListPosY++;
prodList.Print("(" + productList.Sum(Product => Product.ProductPrice) + ")", 3, prodListPosX + 29, prodListPosY);
Console.ResetColor();

// Display class for handling different types of messages
class Display
{
    // Display properties
    public string Message;
    public int MessageType = 0;
    public int PosX = 0;
    public int PosY = 0;
    private int AltPosX = 0;
    private int AltPosY = 0;
    private int showMenu = 0;
    const string BoldText = "\x1b[1m";
    const string NormalText = "\x1b[0m";
    public ConsoleColor TitleColor;
    public ConsoleColor NormalColor;
    public ConsoleColor ErrorColor;
    public ConsoleColor InfoColor;
    public ConsoleColor DoneColor;

    // Constructors
    public Display(ConsoleColor titleColor, ConsoleColor normalColor, ConsoleColor errorColor, ConsoleColor infoColor, ConsoleColor doneColor)
    {
        TitleColor = titleColor;
        NormalColor = normalColor;
        ErrorColor = errorColor;
        InfoColor = infoColor;
        DoneColor = doneColor;
    }
    public Display(ConsoleColor titleColor, ConsoleColor normalColor, ConsoleColor errorColor, ConsoleColor infoColor, ConsoleColor doneColor, int posX, int posY)
    {
        TitleColor = titleColor;
        NormalColor = normalColor;
        ErrorColor = errorColor;
        InfoColor = infoColor;
        DoneColor = doneColor;
        PosX = posX;
        PosY = posY;
    }

    public Display(ConsoleColor titleColor, ConsoleColor normalColor, ConsoleColor infoColor, int posX, int posY)
    {
        PosX = posX;
        PosY = posY;
        TitleColor = titleColor;
        NormalColor = normalColor;
        ErrorColor = ConsoleColor.White;
        InfoColor = infoColor;
        DoneColor = ConsoleColor.Blue;

    }

    // Methods for clearing lines
    public void ClearLine()
    {
        Console.SetCursorPosition(PosX, PosY);
        Console.Write(new String(' ', Console.BufferWidth));
    }

    public void ClearLine(int posX, int posY)
    {
        Console.SetCursorPosition(posX, posY);
        Console.Write(new String(' ', Console.BufferWidth));
    }
/*
    public void ClearLine(int msgType, int posX, int posY)
    {
        Console.SetCursorPosition(PosX, PosY);
        Console.Write(new String(' ', Console.BufferWidth));
    }
*/

    // Metods for set the positions
    public void SetCursurPos(int posX, int posY)
    {
        Console.SetCursorPosition(posX, posY);
    }

    public void SetCursurPos()
    {
        Console.SetCursorPosition(PosX, PosY);
    }

    public void ClearAltCursurPos()
    {
        AltPosX = 0; 
        AltPosY = 0;
    }


    //  Metods to prepare for printing messages Clearline, Set posistions from default posistion
    public void Print(string msg, int msgType)
    {
        if (AltPosX != 0 || AltPosY != 0)
        { 
            ClearLine(AltPosX, AltPosY);
        }
        ClearLine();
        SetCursurPos(PosX, PosY);
        PrintOut(msg, msgType);
        
    }

    //  Metods to prepare for printing messages Clearline, Set posistions from input position
    public void Print(string msg, int msgType, int posX, int posY)
    {
        ClearLine(posX, posY);

        if (msgType > 0 && (AltPosX == 0 && AltPosY == 0))
        {
            AltPosX = posX;
            AltPosY = posY;
/*
            if (AltPosX == 0 && AltPosY == 0)
            {
//                ClearLine(AltPosX, AltPosY);
                AltPosX = posX;
                AltPosY = posY;
            }
*/
        }
        SetCursurPos(posX, posY);
        PrintOut(msg, msgType);
    }

    // Metode for print out from class Product
    public void Print(Product prodList, int msgType, int posX, int posY)
    {
//        int showMenu = 0;
//       SetCursurPos(posX, posY);

        if (this.showMenu == 0)
        {
            SetCursurPos(posX, posY);
            PrintOut("ArtNr".PadRight(10) + "Category".PadRight(10) + "Product".PadRight(10) + "Price", 0);
            this.showMenu = 1;
        }
        SetCursurPos(posX, posY + 1);

        PrintOut(prodList.ProductId.ToString().PadRight(10) + prodList.ProductCategory.PadRight(10) + prodList.ProductName.PadRight(10) + prodList.ProductPrice, msgType);
    }
    /*
     * MsgType 0 = TitelColor
     * MsgType 1 = NormalColor
     * MsgType 2 = ErrorColor
     * MsgType 3 = InfoColor
     * MsgType 4 = DoneColor
     */
    // Metode for set forgroundcolor and print out message

    private void PrintOut(string msg, int msgType)
    {
        switch (msgType)
        {
            case 0:
                Console.ForegroundColor = TitleColor;
                Console.Write(BoldText + msg + NormalText);
                break;
            case 1:
                Console.ForegroundColor = NormalColor;
                Console.Write(msg);
                break;
            case 2:
                Console.ForegroundColor = ErrorColor;
                Console.Write(msg);
                break;
            case 3:
                Console.ForegroundColor = InfoColor;
                Console.Write(msg);
                break;
            case 4:
                Console.ForegroundColor = DoneColor;
                Console.Write(msg);
                break;
        }
        Console.ForegroundColor = NormalColor;

    }

}// Product class to represent a product
class Product
{
    // Constructors
    public Product()
    {
    }

    public Product(int productId, string productCategory, string productName, int productPrice)
    {
        ProductId = productId;
        ProductName = productName;
        ProductCategory = productCategory;
        ProductPrice = productPrice;
    }

    // Get - Set metods for input
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public string ProductCategory { get; set; }
    public int ProductPrice { get; set; }
}