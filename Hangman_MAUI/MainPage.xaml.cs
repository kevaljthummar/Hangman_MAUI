using System.ComponentModel;

namespace Hangman_MAUI;

public partial class MainPage : ContentPage, INotifyPropertyChanged
{
    #region UI Properties
    public string Spotlight
    {
        get => spotlight;
        set
        {
            spotlight = value;
            OnPropertyChanged();
        }
    }

    public List<char> Letters
    {
        get => letters;
        set
        {
            letters = value;
            OnPropertyChanged();
        }
    }

    public string Message
    {
        get => message;
        set
        {
            message = value;
            OnPropertyChanged();
        }
    }

    public string GameStatus
    {
        get => gamestatus;
        set
        {
            gamestatus = value;
            OnPropertyChanged();
        }
    }

    public string CurrentImage
    {
        get => currentimage;
        set
        {
            currentimage = value;
            OnPropertyChanged();
        }
    }


    #endregion

    #region Fields
    List<string> words = new List<string>()
    {
        "python",
        "javascript",
        "maui",
        "csharp",
        "sql",
        "mongodb",
        "xaml",
        "word",
        "excel"
    };
    string answer = "";
    private string spotlight;
    private List<char> guessedList = new List<char>();
    private List<char> letters = new List<char>();
    private string message;
    int mistakes = 0;
    int maxWrong = 6;
    private string gamestatus = string.Empty;
    private string currentimage = "img0.jpg";
    #endregion

    public MainPage()
    {
        InitializeComponent();
        Letters.AddRange("abcdefghijklmnpqrstuvwxyz");
        BindingContext = this;
        PickWord();
        CalculateWord(answer, guessedList);
    }

    #region Game Engine
    private void PickWord()
    {
        answer = words[new Random().Next(0, words.Count)];
    }

    private void CalculateWord(string answer, List<char> guessedList)
    {
        var temp = answer.Select(x => (guessedList.IndexOf(x) => 0 ? x : '_')).ToArray();
        Spotlight = string.Join(' ', temp);
    }
    #endregion

    private void Button_Clicked(object sender, EventArgs e)
    {
        var btn = sender as Button;
        if (btn != null)
        {
            var letter = btn.Text;
            btn.IsEnabled = false;
            HandleGuess(letter[0]);
        }
    }

    private void HandleGuess(char letter)
    {
        if (guessedList.IndexOf(letter) == -1)
        {
            guessedList.Add(letter);
        }
        if (answer.IndexOf(letter) >= 0)
        {
            CalculateWord(answer, guessedList);
            CheckIfGameWon();
        }
        else if (answer.IndexOf(letter) == -1)
        {
            mistakes++;
            UpdateStatus();
            CheckIfGameLost();
            CurrentImage = $"img{mistakes}.jpg";
        }
    }

    private void CheckIfGameLost()
    {
        if (mistakes == maxWrong)
        {
            Message = "You Lost!!";
            DisableLetters();
        }
    }

    private void DisableLetters()
    {
        foreach (var childern in LettersContainer.Children)
        {
            var btn = childern as Button;
            if (btn != null)
            {
                btn.IsEnabled = false;
            }
        }
    }

    private void EnableLetters()
    {
        foreach (var childern in LettersContainer.Children)
        {
            var btn = childern as Button;
            if (btn != null)
            {
                btn.IsEnabled = true;
            }
        }
    }

    private void CheckIfGameWon()
    {
        if (Spotlight.Replace(" ", "") == answer)
        {
            Message = "You Win!";
            DisableLetters();
        }
    }

    private void UpdateStatus()
    {
        GameStatus = $"Error: {mistakes} of {maxWrong}";
    }

    private void Reset_Clicked(object sender, EventArgs e)
    {
        mistakes = 0;
        guessedList = new List<char>();
        CurrentImage = "img0.jpg";
        PickWord();
        CalculateWord(answer, guessedList);
        Message = string.Empty;
        UpdateStatus();
        EnableLetters();
    }
}

