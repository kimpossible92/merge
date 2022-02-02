using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Arrays : MonoBehaviour {
    [SerializeField] OpenAppLevel levelsManager2;
    public static Arrays THIS;
    public int SizeX = 9;
    public int SizeY = 11;
    public Gem[,] gems;
    Gem GetGem1;
    Gem GetGem2;
    public Gem this[int r, int c]
    {
        get { return gems[r, c]; }
        set { gems[r, c] = value; }
    }
    public HitCandy[] ingredientsGems;
    public HitCandy[] ingredientsGems2;
    public HitCandy IngredientCurrent;
    public Gem currentGem;
    // Use this for initialization
    void Start() {
    }
    private void Awake()
    {
        THIS = this;
        gems = new Gem[SizeY, SizeX];
        for (int row = 0; row < SizeY; row++)
        {
            for (int c = 0; c < SizeX; c++)
            {
                gems[row, c] = new Gem(row, c);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void OnSwapping(Gem gem1, Gem gem2)
    {
        GetGem1 = gem1;
        GetGem2 = gem2;

        HitCandy hitGem = gem1.hitGem;
        gem1.OnInit(gem2.hitGem);
        gem2.OnInit(hitGem);
    }
    public void Lastsp()
    {
        OnSwapping(GetGem1, GetGem2);
    }
    public NeighbourProp GetNewProp(Gem gem1,Gem gem2)
    {
        NeighbourProp neighbour = new NeighbourProp();
        IEnumerable<Gem> nhMatch = matchesOnes(gem1,gem2);
        foreach(var g in nhMatch)
        {
            neighbour.gemms.Add(g);
        }
        return neighbour;
    }
    /// <summary>
    /// добавить весь ряд
    /// </summary>
    /// <param name="gem"></param>
    /// <returns></returns>
    public IEnumerable<Gem> GetEntireRow(Gem gem)
    {
        List<Gem> vermatch = new List<Gem> { gem };
        int rower1 = gem.y;
        int col1 = gem.x;
        for (int row = 0; row < SizeY; row++)
        {
            if (IsNulls(row, col1) == false&&gems[row,col1].hitGem.type!= "ingredient"+0&& gems[row, col1].hitGem.type != "ingredient"+1) vermatch.Add(gems[row, col1]);
        }
        return vermatch;
    }
    /// <summary>
    /// добавить всю строку
    /// </summary>
    /// <param name="gem"></param>
    /// <returns></returns>
    public IEnumerable<Gem> GetEntireCol(Gem gem)
    {
        List<Gem> hormatch = new List<Gem> { gem };
        int row1 = gem.y;
        for (int col = 0; col < SizeX; col++)
        {
            if (IsNulls(row1, col) == false&&gems[row1,col].hitGem.type != "ingredient"+0&& gems[row1, col].hitGem.type != "ingredient" + 1) hormatch.Add(gems[row1, col]);
        }
        return hormatch;
    }
    public IEnumerable<Gem> GetEntireAlls(Gem gem)
    {
        List<Gem> matchesall = new List<Gem> { gem};
        int row1 = gem.y;
        int col1 = gem.x;
        for (int row = gem.y - 1; row < gem.y + 2; row++)
        {
            for (int col = gem.x - 1; col < gem.x + 2; col++)
            {
                if (gems[row, col] != null)
                {
                    if(IsNulls(row1, col) == false) matchesall.Add(gems[row, col]);
                }
            }
        }
        return matchesall;
    }
    public IEnumerable<Gem> GetEntire5Match(Gem gem)
    {
        List<Gem> matchesall = new List<Gem> { gem };
        for(int row=0;row<SizeY;row++)
        {
            for(int col=0;col<SizeX;col++)
            {
                if((gems[row,col]).match3(gem))
                {
                    if (IsNulls(row, col) == false) matchesall.Add(gems[row, col]);
                }
            }
        }
        return matchesall;
    }
    private IEnumerable<Gem> matchesOnes(Gem gem1,Gem gem2)
    {
        List<Gem> hormatch = new List<Gem>();
        hormatch.Add(gem1);
        if (gem1.match3(gem2))
        {
            hormatch.Add(gem2);
        }
        if (hormatch.Count() < 2)
        {
            hormatch.Clear();
        }
        return hormatch.Distinct();
    }
    public bool IsNulls(int row,int col)
    {
        if (levelsManager2.blocksp[row*levelsManager2.MaxX+col].types==0)
        {
            return true;
        }
        return false;
    }
    public IEnumerable<Gem> NullGemsonc(int collum)
    {
        List<Gem> gemsnull = new List<Gem>();
        for (int i = 0; i < SizeY; i++)
        {
            if (gems[i, collum].INull||IsNulls(i,collum)==true||levelsManager2.blocksp[i*levelsManager2.MaxX+collum].types==0)
            {
                gemsnull.Add(gems[i, collum]);
            }
        }
        return gemsnull;
    }
    public bool GetBonusType1(IEnumerable<Gem> gemses)
    {
        if(gemses.Count()>=4)
        {
            foreach(var g in gemses)
            {
                if (g.bonus == 0) return true;
            }
        }
        return false;
    }
    /// <summary>
    /// определяем бонус
    /// </summary>
    /// <param name="gemses"></param>
    /// <returns></returns>
    public bool GetBonusType2(IEnumerable<Gem> gemses)
    {
        if(gemses.Count()>=3)
        {
            foreach(var g in gemses)
            {
                if (g.hitGem.isBonus==true) { return true; }
            }
        }
        return false;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="gemses"></param>
    /// <returns></returns>
    public bool GetSwirlType1(IEnumerable<Gem> gemses)
    {
        if(gemses.Count()>=2)
        {
            foreach(var g in gemses)
            {
                if (g.hitGem.isSwirl == true) { return true; }
            }
        }
        return false;
    }
}