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


// Part
int iNumberScratchCards = lstScratchCards.Count;
List<CopiedCards> lstNewCardsToProcess = new List<CopiedCards>();

for (int iCard = 0; iCard < lstScratchCards.Count; iCard++)
{
    for(int iAddition = 1; iAddition <= lstScratchCards[iCard].NumberMatches; iAddition++)
    {
        int iNewCardNumber = iCard + iAddition;
        if (iNewCardNumber < lstScratchCards.Count)
        {
            CopiedCards objCopiedCard = new CopiedCards(lstScratchCards[iNewCardNumber].CardNumber, lstScratchCards[iNewCardNumber].NumberMatches);
            lstNewCardsToProcess.Add(objCopiedCard);
        }
    }
}

iNumberScratchCards += lstNewCardsToProcess.Count;

List<CopiedCards> lstNewCards = new List<CopiedCards>();


int iMinCardNumber = 999999;
do
{
    // clear list of new cards to add new ones for this loop
    lstNewCards.Clear();

    for (int iCard = 0; iCard < lstNewCardsToProcess.Count; iCard++)
    {
        for (int iAddition = 1; iAddition <= lstNewCardsToProcess[iCard].NumberMatches; iAddition++)
        {
            int iNewCardNumber = lstNewCardsToProcess[iCard].CardNumber - 1 + iAddition;
            if (iNewCardNumber < lstScratchCards.Count)
            {
                CopiedCards objCopiedCard = new CopiedCards(lstScratchCards[iNewCardNumber].CardNumber, lstScratchCards[iNewCardNumber].NumberMatches);
                lstNewCards.Add(objCopiedCard);

                if (lstScratchCards[iNewCardNumber].CardNumber < iMinCardNumber)
                {
                    iMinCardNumber = lstScratchCards[iNewCardNumber].CardNumber;
                }
            }
        }
    }

    iNumberScratchCards += lstNewCards.Count;

    // repopulate list of cards to process and add new cards for next loop
    lstNewCardsToProcess.Clear();

    // Add new cards
    foreach (var item in lstNewCards)
    {
        lstNewCardsToProcess.Add(item);
    }

} while (lstNewCardsToProcess.Count > 0);


Console.WriteLine("Part 1 score is :" + iNumberScratchCards);


/// <summary>
///  Did this the hard way!
///
/// Should have just looped around once and increments x records with the matching count and then summed them!
/// </summary>


public class CopiedCards
{
    public int CardNumber;
    public int NumberMatches;

    public CopiedCards(int iCardNumber, int iNumberMatches)
    {
        CardNumber = iCardNumber;
        NumberMatches = iNumberMatches;
    }
}



public class Scratchcard
{
    public int CardNumber;
    public List<int> WinningNumbers = new List<int>();
    public List<int> YourNumbers = new List<int>();
    public int Score;
    public int NumberMatches;
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
        objScratchcard.NumberMatches = iNumberMatches;

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