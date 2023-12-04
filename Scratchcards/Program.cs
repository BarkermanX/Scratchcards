// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");


// Specify the path to your text file
string filePath = "TestData.txt";

List<Scratchcard> lstScratchCards = new List<Scratchcard>();

// Check if the file exists
if (File.Exists(filePath))
{
    // Use a StreamReader to read the file
    using (StreamReader reader = new StreamReader(filePath))
    {
        string line;

        // Read lines until the end of the file is reached
        while ((line = reader.ReadLine()) != null)
        {
            Scratchcard objScratchcard = Helper.getScratchcardFromDataLine(line);
            lstScratchCards.Add(objScratchcard);
        }
    }
}
else
{
    Console.WriteLine("File not found: " + filePath);
}


// Loop through the list
int iSum = 0;
foreach (Scratchcard objScratchcard in lstScratchCards)
{
    iSum += objScratchcard.Score;
}

Console.WriteLine("Part 1 score is :" + iSum);




public class Scratchcard
{
    public int CardNumber;
    public List<int> WinningNumbers = new List<int>();
    public List<int> YourNumbers = new List<int>();
    public int Score;
}


public static class Helper
{
    public static bool getResult()
    {
        return true;
    }

    public static Scratchcard getScratchcardFromDataLine(string strDataLine)
    {
        // Test Line
        //"Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53"

        Scratchcard objScratchcard = new Scratchcard();

        // Card Number
        string[] astrCardNumber = strDataLine.Split(':');
        astrCardNumber[0] = astrCardNumber[0].Replace("Card ", "");
        int iCardNumber = int.Parse(astrCardNumber[0]);

        objScratchcard.CardNumber = iCardNumber;

        // Data
        string[] astrCardData = astrCardNumber[1].Split('|');
        astrCardData = astrCardData.Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();


        // Winning Numbers
        string[] astrWinningNumbers = astrCardData[0].Split(' ');
        astrWinningNumbers = astrWinningNumbers.Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();


        foreach (string strWinningNum in astrWinningNumbers)
        {
            int iWinningNumber = int.Parse(strWinningNum);
            objScratchcard.WinningNumbers.Add(iWinningNumber);
        }

        // Your Numbers
        string[] astrYourNumbers = astrCardData[1].Split(' ');
        astrYourNumbers = astrYourNumbers.Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();


        foreach (string strYourNum in astrYourNumbers)
        {
            int iYourNumber = int.Parse(strYourNum);
            objScratchcard.YourNumbers.Add(iYourNumber);
        }

        objScratchcard.Score = Helper.getCardScore(objScratchcard);

        return objScratchcard;
    }

    public static int getCardScore(Scratchcard objScratchcard)
    {
        // Matching numbers
        List<int> lstMatchingNumbers = objScratchcard.WinningNumbers.Intersect(objScratchcard.YourNumbers).ToList();

        // Number of matches
        int iNumberMatches = lstMatchingNumbers.Count;

        int iScore = 0;

        if (iNumberMatches > 0)
        {
            iScore = 1;
            // Loop through the desired number of iterations
            for (int i = 1; i < iNumberMatches; i++)
            {
                // Double the score
                iScore *= 2;
            }
        }

        return iScore;
    }
}