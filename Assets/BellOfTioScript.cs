using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using Rnd = UnityEngine.Random;

public class BellOfTioScript : MonoBehaviour
{
    public KMBombModule Module;
    public KMBombInfo BombInfo;
    public KMAudio Audio;

    public KMSelectable BellSel;
    public GameObject BellRingerObj;
    public GameObject BellMainObj;

    public TextMesh TempText;

    private int _moduleId;
    private static int _moduleIdCounter = 1;
    private bool _moduleSolved;

    private static readonly string[] _wordList = new string[]
    {
        // None of these words have duplicate letters.
        // All of these words ensure that all five rules will be triggered by at least one of the rows.
        "ABORT", "ABOUT", "ABOVE", "ACIDS", "ACORN", "ACTOR", "ADIOS", "ADOPT", "ADORE", "ADORN", "AGENT", "AGONY", "AIRED", "AISLE", "ALBUM", "ALERT", "ALIVE", "ALOFT", "ALOUD", "ALTER", "AMBER", "AMISH", "AMUSE", "ANGER", "ANGRY", "ANGST", "ANISE", "ANTIC", "ANVIL", "APRON", "ARISE", "ARMED", "AROSE", "ARSON", "ASHEN", "ASIDE", "ASKED", "ASTIR", "ATOMS", "ATONE", "AUDIO", "AUNTY", "AVOID", "AWFUL", "AWOKE", "AXIOM", "AXION", "AZTEC", "BADLY", "BAKER", "BANDS", "BANKS", "BARON", "BASIL", "BASIN", "BATCH", "BATHS", "BEAMS", "BEANS", "BEGUN", "BELOW", "BELTS", "BIDET", "BIGHT", "BIKES", "BIRDS", "BIRTH", "BISON", "BITCH", "BITER", "BLARE", "BLAST", "BLAZE", "BLEAT", "BLIMP", "BLITZ", "BLOWN", "BLOWS", "BLUES", "BLUNT", "BLUSH", "BOARD", "BOATS", "BOGUS", "BOLTS", "BONDS", "BONER", "BONES", "BONUS", "BORAX", "BORED", "BORNE", "BOTCH", "BOUGH", "BOULE", "BOUND", "BOWED", "BOWEL", "BOWLS", "BOXED", "BOXER", "BOXES", "BRAID", "BRAIN", "BRAKE", "BRAND", "BRAWL", "BRAWN", "BRAZE", "BREAK", "BREAM", "BRIDE", "BRIEF", "BRINE", "BRING", "BRINK", "BRINY", "BROAD", "BROIL", "BROKE", "BROTH", "BROWN", "BROWS", "BRUNT", "BRUSH", "BUCKS", "BUILD", "BUILT", "BULGE", "BULKY", "BUMPY", "BUNCH", "BURNS", "BURNT", "BUYER", "BYLAW", "CAIRN", "CAKES", "CALVE", "CALYX", "CAMPS", "CAMPY", "CANDY", "CARGO", "CAROL", "CAULK", "CENTS", "CHAMP", "CHANT", "CHAOS", "CHARM", "CHART", "CHASE", "CHASM", "CHEAP", "CHEAT", "CHEST", "CHOIR", "CHORD", "CHORE", "CHOSE", "CHUNK", "CHUTE", "CIDER", "CIGAR", "CITED", "CITES", "CIVET", "CLASH", "CLAWS", "CLEAR", "CLEAT", "CLERK", "CLOSE", "CLOTH", "CLOUD", "CLOUT", "CLOVE", "CLOWN", "CLUBS", "CLUES", "CLUNG", "CLUNK", "COAST", "COATS", "CODES", "COINS", "COMES", "CORAL", "CORGI", "CORNY", "CORPS", "COUGH", "COULD", "COUNT", "COURT", "COVEN", "COVER", "CRANE", "CRANK", "CRAWL", "CREAK", "CREAM", "CRIED", "CRIES", "CRIME", "CRONE", "CROPS", "CROWD", "CROWN", "CRUEL", "CRUSH", "CRYPT", "CUBAN", "CUBIT", "CUMIN", "CURLS", "CURLY", "CUTIE", "DAIRY", "DAISY", "DEALS", "DEBIT", "DECAY", "DECOR", "DECOY", "DEIST", "DELFT", "DEMUR", "DEPOT", "DERBY", "DETOX", "DEVIL", "DIARY", "DIETS", "DINAR", "DINER", "DINGY", "DIRTY", "DISCO", "DITCH", "DITZY", "DIVAN", "DIVER", "DIVOT", "DOCKS", "DONUT", "DORIC", "DOUBT", "DOUGH", "DOUSE", "DOWNS", "DOZEN", "DRAIN", "DRANK", "DRAWN", "DREAM", "DRIFT", "DRILY", "DRINK", "DRIVE", "DRONE", "DROPS", "DROVE", "DROWN", "DRUMS", "DRUNK", "DUCHY", "DUCKS", "DUNCE", "DUNES", "DUSTY", "DUTCH", "DYING", "EARLY", "EARTH", "ECLAT", "EDICT", "EDIFY", "EIGHT", "ELBOW", "ENACT", "ENJOY", "ENTRY", "ENVOY", "ETHIC", "ETHOS", "EVICT", "EXACT", "EXAMS", "EXIST", "EXTRA", "FAILS", "FAINT", "FAIRS", "FAIRY", "FAITH", "FALSE", "FANCY", "FARMS", "FAULT", "FAVOR", "FEINT", "FETAL", "FETCH", "FIBER", "FIBRE", "FIERY", "FIGHT", "FILES", "FILET", "FILMS", "FILMY", "FILTH", "FINDS", "FINER", "FINES", "FIRED", "FIRES", "FIRMS", "FIRST", "FIVER", "FIXED", "FLAGS", "FLAIR", "FLARE", "FLASH", "FLASK", "FLATS", "FLAWS", "FLESH", "FLIES", "FLIRT", "FLOAT", "FLORA", "FLOUR", "FLOUT", "FLOWN", "FLOWS", "FLUID", "FLUNG", "FLUNK", "FLUSH", "FLUTE", "FOCUS", "FOIST", "FOLDS", "FOLKS", "FONTS", "FORAY", "FORCE", "FORGE", "FORMS", "FORTE", "FORTH", "FORTY", "FORUM", "FOUND", "FOUNT", "FOURS", "FOVEA", "FOXES", "FOYER", "FRAIL", "FRAME", "FRANC", "FRANK", "FREAK", "FRESH", "FRIED", "FRISK", "FROGS", "FRONT", "FROST", "FROWN", "FROZE", "FRUIT", "FUELS", "FUMES", "FUNDS", "FUTON", "GAINS", "GAMES", "GAZED", "GENUS", "GHOST", "GHOUL", "GIANT", "GIFTS", "GIMPY", "GIRLS", "GIRLY", "GIRTH", "GIVEN", "GIVES", "GIZMO", "GLARE", "GLINT", "GLORY", "GLOVE", "GLUED", "GLUON", "GOALS", "GOATS", "GRAIN", "GRAMS", "GRAND", "GRANT", "GRAPH", "GRAVY", "GRIEF", "GRIME", "GRIMY", "GRIND", "GRIPS", "GROIN", "GROUP", "GROUT", "GROWN", "GROWS", "GRUEL", "GRUMP", "GRUNT", "GUANO", "GUIDE", "GUILD", "GUILT", "GUISE", "GUNKY", "GUSHY", "GUSTY", "GUTSY", "GYRUS", "HABIT", "HAIKU", "HALVE", "HANDS", "HANDY", "HANGS", "HARDY", "HAREM", "HASTE", "HASTY", "HATES", "HAUNT", "HAVEN", "HAVOC", "HAZEL", "HEADS", "HEARD", "HEARS", "HEART", "HEAVY", "HEFTY", "HEIRS", "HEIST", "HELPS", "HENRY", "HERBS", "HERDS", "HINDU", "HINTS", "HIRED", "HOIST", "HOLDS", "HOLES", "HOMES", "HONEY", "HOPED", "HOPES", "HORNS", "HORSE", "HOSEL", "HOTEL", "HOTLY", "HOUND", "HOURS", "HOUSE", "HUMAN", "HUMOR", "HURTS", "HUSKY", "HYENA", "HYMNS", "ICHOR", "ICONS", "IDEAS", "IMBUE", "INCUS", "INDEX", "INERT", "INFER", "INFRA", "INGOT", "INLAY", "INLET", "INPUT", "INTRO", "IRONY", "ISLET", "ITCHY", "ITEMS", "IVORY", "JEANS", "JOINS", "JOINT", "JOKER", "JOKES", "JOULE", "JOUST", "JUDGE", "JUICE", "JUICY", "JUMBO", "JUMPS", "JUNTA", "KINDS", "KINGS", "KNAVE", "KNELT", "KNOBS", "KNOTS", "KNOWS", "KUDOS", "LABOR", "LACKS", "LAGER", "LAKES", "LAMBS", "LAMPS", "LANDS", "LANES", "LAPIN", "LAPSE", "LARGE", "LASER", "LATCH", "LATER", "LATIN", "LAUGH", "LAWNS", "LAYER", "LEADS", "LEAFY", "LEAKY", "LEANT", "LEARN", "LEASH", "LEAST", "LEMUR", "LIDAR", "LIFTS", "LIGHT", "LIKES", "LIMBS", "LINER", "LINES", "LINKS", "LIONS", "LITER", "LITRE", "LIVED", "LIVEN", "LIVER", "LIVES", "LOADS", "LOANS", "LOCKS", "LOCUS", "LOFTY", "LONER", "LORDS", "LOSER", "LOTUS", "LOUSE", "LOUSY", "LOVED", "LOVER", "LOVES", "LOWER", "LUCID", "LUCKY", "LUCRE", "LUMEN", "LUMPS", "LUNAR", "LUNCH", "LUNGE", "LUNGS", "LUSTY", "LYING", "LYNCH", "LYRIC", "MAINS", "MAIZE", "MAJOR", "MAKER", "MAKES", "MALES", "MANGY", "MANLY", "MANOR", "MARCH", "MARKS", "MARSH", "MATCH", "MATES", "MATHS", "MAVEN", "MAYBE", "MAYOR", "MEALS", "MEANS", "MEANT", "MENUS", "MERCY", "MERIT", "MESON", "MICRO", "MIDST", "MIGHT", "MILES", "MINDS", "MINER", "MINES", "MINOR", "MINTY", "MINUS", "MIRED", "MIRTH", "MISTY", "MITRE", "MIXER", "MODES", "MOGUL", "MOIST", "MOLAR", "MOLDY", "MOLES", "MONEY", "MONKS", "MONTH", "MORAL", "MORAY", "MORPH", "MOTEL", "MOTIF", "MOULD", "MOUND", "MOUNT", "MOUSE", "MOUTH", "MOVED", "MOVER", "MOVES", "MOVIE", "MUNCH", "MURKY", "MUSED", "MUSIC", "MUSTY", "MYTHS", "NADIR", "NAILS", "NAIVE", "NAMES", "NASTY", "NAVEL", "NEATH", "NECKS", "NEWLY", "NEXUS", "NICER", "NIFTY", "NIGHT", "NITRO", "NOBLY", "NODES", "NOISE", "NOISY", "NOMES", "NORMS", "NORTH", "NOTCH", "NOTED", "NOTES", "NOVEL", "NUDGE", "NURSE", "NYMPH", "OFTEN", "OLDER", "OLIVE", "ONSET", "OPENS", "OPERA", "OPINE", "OPIUM", "OPTIC", "ORBIT", "ORGAN", "OTHER", "OUGHT", "OUNCE", "OUTER", "OVERS", "OVERT", "OVULE", "OWNED", "OWNER", "OXIDE", "PAINS", "PAINT", "PALMS", "PANEL", "PANIC", "PANTS", "PARTY", "PATCH", "PATHS", "PATIO", "PEACH", "PEAKS", "PEARL", "PENAL", "PERIL", "PHASE", "PHONE", "PIANO", "PIERS", "PILAF", "PILES", "PILOT", "PINCH", "PINTS", "PIOUS", "PISTE", "PITCH", "PIVOT", "PLACE", "PLAIN", "PLANE", "PLANK", "PLANS", "PLANT", "PLAYS", "PLEAS", "PLOTS", "PLUMB", "POEMS", "POETS", "POINT", "POKER", "POLAR", "POLES", "PONDS", "PORCH", "PORES", "PORTS", "POSED", "POSIT", "POUCH", "POUND", "POWER", "PRICE", "PRIDE", "PRIMA", "PRIME", "PRINT", "PRION", "PRISE", "PRISM", "PRIVY", "PRIZE", "PROBE", "PRONE", "PRONG", "PROSE", "PROUD", "PROVE", "PROXY", "PRUNE", "PUDGY", "PULSE", "PUNCH", "PYLON", "QUACK", "QUARK", "QUASI", "QUERY", "QUICK", "QUINT", "QUIRK", "QUOTA", "QUOTE", "RADIO", "RAIDS", "RAILS", "RAINY", "RAISE", "RAMPS", "RANCH", "RANGE", "RANGY", "RANKS", "RAPID", "RATIO", "RAVEN", "REACH", "READY", "REALM", "RECON", "RECTO", "REDLY", "REHAB", "REIGN", "REINS", "RELAX", "RELAY", "RELIC", "REMIT", "REMIX", "RENAL", "RENTS", "REPAY", "REPLY", "RESIN", "RHINO", "RHYME", "RIDGE", "RIFLE", "RIGHT", "RILED", "RINGS", "RINSE", "RIOTS", "RISEN", "RITES", "RITZY", "RIVAL", "RIVEN", "RIVET", "ROADS", "ROAST", "ROBES", "ROCKS", "ROCKY", "ROGUE", "ROILY", "ROLES", "ROMAN", "ROPES", "ROSIN", "ROUGE", "ROUGH", "ROUND", "ROUTE", "ROYAL", "RUGBY", "RUINS", "RULED", "RULES", "RUMBA", "RUNIC", "RUNTY", "RUSTY", "SABLE", "SADLY", "SAINT", "SALON", "SALTY", "SALVE", "SANDY", "SATIN", "SATYR", "SAUCY", "SAVOR", "SCALD", "SCALE", "SCALP", "SCALY", "SCAMP", "SCANT", "SCENT", "SCHMO", "SCOLD", "SCONE", "SCOPE", "SCORE", "SCORN", "SCOUR", "SCOUT", "SCRAM", "SCRIM", "SCRUM", "SERUM", "SHADE", "SHADY", "SHAFT", "SHAKE", "SHALE", "SHAME", "SHANK", "SHAPE", "SHARD", "SHARE", "SHAVE", "SHAWL", "SHEAF", "SHEAR", "SHELF", "SHIFT", "SHINE", "SHINY", "SHIRE", "SHIRT", "SHOCK", "SHONE", "SHORE", "SHORN", "SHORT", "SHOUT", "SHOVE", "SHOWN", "SHRUG", "SHUNT", "SIDLE", "SIGHT", "SILTY", "SINCE", "SINEW", "SINGE", "SITAR", "SIXTH", "SIXTY", "SIZED", "SKALD", "SKATE", "SKEIN", "SKIER", "SKIMP", "SKIRT", "SLAIN", "SLAKE", "SLANG", "SLANT", "SLATE", "SLAVE", "SLEPT", "SLICE", "SLIDE", "SLIME", "SLIMY", "SLING", "SLINK", "SLOPE", "SLOTH", "SLUMP", "SMART", "SMEAR", "SMELT", "SMILE", "SMITE", "SMOKE", "SNAIL", "SNAKE", "SNARE", "SNARL", "SNIDE", "SNIPE", "SNORE", "SNORT", "SNOUT", "SOBER", "SOFTY", "SOLAR", "SOLID", "SOLVE", "SONAR", "SONIC", "SOUGH", "SOUND", "SOUTH", "SPAIN", "SPAWN", "SPEAK", "SPEND", "SPENT", "SPINE", "SPLAT", "SPLIT", "SPOIL", "SPOKE", "SPORT", "STACK", "STAIN", "STAIR", "STAKE", "STALE", "STAMP", "STAND", "STARK", "STEAK", "STEAL", "STEAM", "STENO", "STERN", "STICK", "STILE", "STING", "STINK", "STOCK", "STOIC", "STOKE", "STOLE", "STOMP", "STONE", "STONY", "STORE", "STORK", "STORM", "STORY", "STOVE", "STRAY", "STRIP", "STRUM", "STUCK", "STUDY", "STUMP", "STYLE", "SUITE", "SWALE", "SWAMI", "SWAMP", "SWANK", "SWARM", "SWATH", "SWIFT", "SWINE", "SWING", "SWIPE", "SWIRL", "SWORD", "SWORE", "SWORN", "SWUNG", "TABLE", "TAILS", "TAKEN", "TAKES", "TALES", "TALKS", "TALON", "TANGO", "TANGY", "TANKS", "TARDY", "TAWNY", "TAXES", "TAXIS", "TAXON", "TEACH", "TEAMS", "TEARY", "TECHY", "TEMPO", "TENDS", "TENOR", "TERMS", "TEXAS", "THANK", "THEIR", "THICK", "THIEF", "THINE", "THING", "THINK", "THIRD", "THONG", "THORN", "THOSE", "THREW", "THROW", "THUMB", "TIDES", "TIGER", "TILES", "TIMER", "TIMES", "TINES", "TIPSY", "TIRED", "TODAY", "TOKEN", "TONAL", "TONED", "TONES", "TONGS", "TONIC", "TOPAZ", "TOPIC", "TORCH", "TORUS", "TOUCH", "TOUGH", "TOURS", "TOWEL", "TOWER", "TOWNS", "TOXIC", "TOXIN", "TRACK", "TRAIL", "TRAIN", "TRAMP", "TRAMS", "TRASH", "TRAWL", "TRAYS", "TREND", "TRIAD", "TRIAL", "TRIBE", "TRICK", "TRIED", "TRIES", "TRIKE", "TRIPS", "TRUCK", "TRULY", "TRUNK", "TUNED", "TUNES", "TUNIC", "TURKS", "TURNS", "TWANG", "TWEAK", "TWICE", "TWINS", "TWIRL", "TYING", "TYPES", "TYRES", "ULCER", "ULNAR", "ULTRA", "UMBRA", "UNCAP", "UNCLE", "UNDER", "UNFED", "UNFIT", "UNHIP", "UNIFY", "UNITE", "UNITS", "UNITY", "UNLIT", "UNMET", "UNSAY", "UNTIE", "UNTIL", "UNZIP", "URBAN", "URINE", "USHER", "USING", "VALET", "VALID", "VALOR", "VALUE", "VAPOR", "VAULT", "VAUNT", "VEINS", "VEINY", "VENAL", "VENOM", "VICAR", "VIDEO", "VIEWS", "VIGOR", "VINES", "VINYL", "VIRAL", "VIRUS", "VISOR", "VITAL", "VIXEN", "VOCAL", "VODKA", "VOGUE", "VOICE", "VOTED", "VOTER", "VOTES", "VOUCH", "VOWED", "VOWEL", "WAGON", "WAIST", "WAITS", "WAIVE", "WALKS", "WALTZ", "WANTS", "WARNS", "WATCH", "WAXEN", "WEARY", "WEIGH", "WEIRD", "WELSH", "WETLY", "WHALE", "WHEAT", "WHILE", "WHINE", "WHISK", "WHITE", "WHOLE", "WHORL", "WHOSE", "WIDEN", "WIDER", "WIDTH", "WIELD", "WIMPY", "WINCE", "WINCH", "WINDS", "WINDY", "WINES", "WINGS", "WIPED", "WIRED", "WIRES", "WISER", "WITCH", "WIVES", "WOKEN", "WOMAN", "WOMEN", "WORDS", "WORKS", "WORLD", "WORMS", "WORMY", "WORSE", "WORST", "WORTH", "WOULD", "WOUND", "WOVEN", "WRATH", "WRECK", "WRIST", "WRITE", "WRONG", "WROTE", "YACHT", "YARDS", "YAWNS", "YEARN", "YEARS", "YEAST", "YODEL", "YOUNG", "YOURS", "YOUTH", "ZEBRA", "ZILCH", "ZINGY", "ZONAL", "ZONES"
    };
    private static readonly string[] _voices = new string[] { "Quinn" };
    private string _voice;

    public class ModificationValue
    {
        public bool IsRingClockwise;
        public int RingCycleCount;
        public bool IsGridClockwise;
        public int[] RowFlips;
        public int[] ColumnFlips;
        public int[] RowCycles;
        public int RowCycleCount;
        public int[] ColumnCycles;
        public int ColumnCycleCount;
        public int[] RowsToSwap;
        public int[] ColumnsToSwap;
        public int[] RandomPositionsToCycle;
        public int RowAndColumnSwapPos;

        public ModificationValue(bool isRingClockwise, int ringCycleCount, bool isGridClockwise, int[] rowFlips, int[] columnFlips, int[] rowCycles, int rowCycleCount, int[] columnCycles, int columnCycleCount, int[] rowsToSwap, int[] columnsToSwap, int[] randomPositionsToCycle, int rowAndColumnSwapPos)
        {
            IsRingClockwise = isRingClockwise;
            RingCycleCount = ringCycleCount;
            IsGridClockwise = isGridClockwise;
            RowFlips = rowFlips;
            ColumnFlips = columnFlips;
            RowCycles = rowCycles;
            RowCycleCount = rowCycleCount;
            ColumnCycles = columnCycles;
            ColumnCycleCount = columnCycleCount;
            RowsToSwap = rowsToSwap;
            ColumnsToSwap = columnsToSwap;
            RandomPositionsToCycle = randomPositionsToCycle;
            RowAndColumnSwapPos = rowAndColumnSwapPos;
        }

        public ModificationValue GenerateModificationValue()
        {
            var randRows = Enumerable.Range(0, 5).ToArray().Shuffle();
            var randCols = Enumerable.Range(0, 5).ToArray().Shuffle();
            return new ModificationValue(
                IsRingClockwise = Rnd.Range(0, 2) == 0,
                RingCycleCount = Rnd.Range(1, 5),
                IsGridClockwise = Rnd.Range(0, 2) == 0,
                RowFlips = Enumerable.Range(0, 5).ToArray().Shuffle().Take(Rnd.Range(1, 4)).ToArray(),
                ColumnFlips = Enumerable.Range(0, 5).ToArray().Shuffle().Take(Rnd.Range(1, 4)).ToArray(),
                RowCycles = Enumerable.Range(0, 5).ToArray().Shuffle().Take(Rnd.Range(1, 4)).ToArray(),
                RowCycleCount = Rnd.Range(1, 4),
                ColumnCycles = Enumerable.Range(0, 5).ToArray().Shuffle().Take(Rnd.Range(1, 4)).ToArray(),
                ColumnCycleCount = Rnd.Range(1, 4),
                RowsToSwap = Enumerable.Range(0, 5).ToArray().Shuffle().Take(2).ToArray(),
                ColumnsToSwap = Enumerable.Range(0, 5).ToArray().Shuffle().Take(2).ToArray(),
                RandomPositionsToCycle = new int[] { randRows[0] * 5 + randCols[0], randRows[1] * 5 + randCols[1], randRows[2] * 5 + randCols[2], randRows[3] * 5 + randCols[3], randRows[4] * 5 + randCols[4] },
                RowAndColumnSwapPos = Rnd.Range(0, 5)
            );
        }
    }

    private readonly ModificationValue _modificationValue = new ModificationValue(false, -1, false, null, null, null, -1, null, -1, null, null, null, -1);

    private string _solutionWord;
    private string _input = "";
    private string _letterGrid;

    private Coroutine _bellPressAnim;
    private Coroutine _readingGrid;
    private Coroutine _spamTimer;

    private ReadingState _readingState;
    private int _gridPos = -5;
    private bool _inputLocked;
    private bool _isInteracting;

    private readonly int[][] _transformations = new int[5][];
    private GridModificationType[] _modificationsToPickFrom;

    enum ReadingState
    {
        Inactive,
        DownTheColumn,
        AlongTheRow
    }

    enum GridModificationType
    {
        GridClockwise,
        RowColumnSwap,
        RowFlip,
        TwoColumnsSwap,
        RowCycle,
        FiveRandomPositionsCycle,
        ColumnCycle,
        TwoRowsSwap,
        ColumnFlip,
        RingCycle
    }

    private void Start()
    {
        _moduleId = _moduleIdCounter++;
        BellSel.OnInteract += BellPress;
        BellSel.OnInteractEnded += BellRelease;

        // Temp text
        TempText.gameObject.SetActive(false);

        _modificationValue.GenerateModificationValue();
        _solutionWord = _wordList.PickRandom();

        _modificationsToPickFrom = Enumerable.Range(0, Enum.GetValues(typeof(GridModificationType)).Length).Select(i => (GridModificationType)i).ToArray().Shuffle().Take(5).OrderBy(i => i).ToArray();

        for (int i = 0; i < _transformations.Length; i++)
            _transformations[i] = ToBinaryArray(_solutionWord[i] - 'A' + 1);
        _voice = _voices.PickRandom();
        var missingLetter = Enumerable.Range(0, 26).Where(i => !_solutionWord.Select(letter => letter - 'A').Contains(i)).PickRandom();
        _letterGrid = Enumerable.Range(0, 26).Except(new[] { missingLetter }).ToArray().Shuffle().Select(i => "ABCDEFGHIJKLMNOPQRSTUVWXYZ"[i]).Join("");

        Debug.LogFormat("[Bell of Tío #{0}] The hidden word is {1}.", _moduleId, _solutionWord);
        Debug.LogFormat("[Bell of Tío #{0}] The letter missing from the grid is {1}.", _moduleId, "ABCDEFGHIJKLMNOPQRSTUVWXYZ"[missingLetter]);
        Debug.LogFormat("[Bell of Tío #{0}] The initial state of the letter grid is {1}.", _moduleId, _letterGrid);

        int logIx = 0;
        var ordinals = new[] { "first", "second", "third", "fourth", "fifth" };
        if (_modificationsToPickFrom.Contains(GridModificationType.GridClockwise))
        {
            Debug.LogFormat("[Bell of Tío #{0}] The {1} modification will rotate the entire grid {2}clockwise.",
                _moduleId, ordinals[logIx], _modificationValue.IsGridClockwise ? "" : "counter");
            logIx++;
        }
        if (_modificationsToPickFrom.Contains(GridModificationType.RowColumnSwap))
        {
            Debug.LogFormat("[Bell of Tío #{0}] The {1} modification will swap Row #{2} with Column #{2}.",
                _moduleId, ordinals[logIx], _modificationValue.RowAndColumnSwapPos + 1);
            logIx++;
        }
        if (_modificationsToPickFrom.Contains(GridModificationType.RowFlip))
        {
            Debug.LogFormat("[Bell of Tío #{0}] The {1} modification will horizontally flip Row{2} {3}.",
                _moduleId, ordinals[logIx], _modificationValue.RowFlips.Count() == 1 ? "" : "s", _modificationValue.RowFlips.OrderBy(i => i).Select(i => i + 1).Join(", "));
            logIx++;
        }
        if (_modificationsToPickFrom.Contains(GridModificationType.TwoColumnsSwap))
        {
            Debug.LogFormat("[Bell of Tío #{0}] The {1} modification will swap Columns #{2} and #{3}.",
                _moduleId, ordinals[logIx], _modificationValue.ColumnsToSwap.OrderBy(i => i).First() + 1, _modificationValue.ColumnsToSwap.OrderBy(i => i).Last() + 1);
            logIx++;
        }
        if (_modificationsToPickFrom.Contains(GridModificationType.RowCycle))
        {
            Debug.LogFormat("[Bell of Tío #{0}] The {1} modification will cycle Row{2} {3} rightwards by {4} spaces.",
                _moduleId, ordinals[logIx], _modificationValue.RowCycles.Count() == 1 ? "" : "s", _modificationValue.RowCycles.OrderBy(i => i).Select(i => i + 1).Join(", "), _modificationValue.RowCycleCount);
            logIx++;
        }
        if (_modificationsToPickFrom.Contains(GridModificationType.FiveRandomPositionsCycle))
        {
            Debug.LogFormat("[Bell of Tío #{0}] The {1} modification will cycle Positions {2} once.",
                _moduleId, ordinals[logIx], _modificationValue.RandomPositionsToCycle.Select(i => i + 1).Join(", "));
            logIx++;
        }
        if (_modificationsToPickFrom.Contains(GridModificationType.ColumnCycle))
        {
            Debug.LogFormat("[Bell of Tío #{0}] The {1} modification will cycle Column{2} {3} downwards by {4} spaces.",
                _moduleId, ordinals[logIx], _modificationValue.ColumnCycles.Count() == 1 ? "" : "s", _modificationValue.ColumnCycles.OrderBy(i => i).Select(i => i + 1).Join(", "), _modificationValue.ColumnCycleCount);
            logIx++;
        }
        if (_modificationsToPickFrom.Contains(GridModificationType.TwoRowsSwap))
        {
            Debug.LogFormat("[Bell of Tío #{0}] The {1} modification will swap Rows #{2} and #{3}.",
                _moduleId, ordinals[logIx], _modificationValue.RowsToSwap.OrderBy(i => i).First() + 1, _modificationValue.RowsToSwap.OrderBy(i => i).Last() + 1);
            logIx++;
        }
        if (_modificationsToPickFrom.Contains(GridModificationType.ColumnFlip))
        {
            Debug.LogFormat("[Bell of Tío #{0}] The {1} modification will vertically flip Column{2} {3}.",
                _moduleId, ordinals[logIx], _modificationValue.ColumnFlips.Count() == 1 ? "" : "s", _modificationValue.ColumnFlips.OrderBy(i => i).Select(i => i + 1).Join(", "));
            logIx++;
        }
        if (_modificationsToPickFrom.Contains(GridModificationType.RingCycle))
        {
            Debug.LogFormat("[Bell of Tío #{0}] The {1} modification will cycle the outer ring {2}clockwise by {3} spaces.",
                _moduleId, ordinals[logIx], _modificationValue.IsRingClockwise ? "" : "counter", _modificationValue.RingCycleCount);
            logIx++;
        }
    }

    private void BellRelease()
    {
        if (_bellPressAnim != null)
            StopCoroutine(_bellPressAnim);
        _bellPressAnim = StartCoroutine(AnimateBellRinger(false));
    }

    private int[] ToBinaryArray(int n)
    {
        var list = new List<int>();
        if (n % 32 / 16 == 1)
            list.Add(0);
        if (n % 16 / 8 == 1)
            list.Add(1);
        if (n % 8 / 4 == 1)
            list.Add(2);
        if (n % 4 / 2 == 1)
            list.Add(3);
        if (n % 2 == 1)
            list.Add(4);
        return list.ToArray();
    }

    private bool BellPress()
    {
        if (_bellPressAnim != null)
            StopCoroutine(_bellPressAnim);
        _bellPressAnim = StartCoroutine(AnimateBellRinger(true));
        Audio.PlaySoundAtTransform("Ring", transform);
        BellSel.AddInteractionPunch(0.25f);

        if (_inputLocked || _moduleSolved)
            return false;

        if (_readingState == ReadingState.Inactive)
        {
            if (_spamTimer != null)
                StopCoroutine(_spamTimer);
            _spamTimer = StartCoroutine(SpamTimer());
            _isInteracting = true;
        }
        else if (_readingState == ReadingState.DownTheColumn)
        {
            _readingState = ReadingState.AlongTheRow;
            if (_readingGrid != null)
                StopCoroutine(_readingGrid);
            _readingGrid = StartCoroutine(ReadGrid());
        }
        else if (_readingState == ReadingState.AlongTheRow)
        {
            if (_readingGrid != null)
                StopCoroutine(_readingGrid);
            if (_spamTimer != null)
                StopCoroutine(_spamTimer);
            _spamTimer = StartCoroutine(SpamTimer());
        }
        return false;
    }

    private IEnumerator ReadGrid()
    {
        if (_readingState == ReadingState.AlongTheRow)
            yield return new WaitForSeconds(1.5f);

        for (int i = 0; i < (_readingState == ReadingState.DownTheColumn ? 5 : 4); i++)
        {
            if (_readingState == ReadingState.DownTheColumn)
                _gridPos += 5;
            else if (_readingState == ReadingState.AlongTheRow)
                _gridPos += 1;
            var letter = _letterGrid[_gridPos];
            ReadLetter(letter);
            TempText.text = letter.ToString();
            yield return new WaitForSeconds(1.15f);
        }
        if (_readingState == ReadingState.DownTheColumn)
        {
            Debug.LogFormat("[Bell of Tío #{0}] Input has been reset.", _moduleId);
            _input = "";
            TempText.text = "reset";
            Audio.PlaySoundAtTransform(_voice + "_Reset", transform);
            _inputLocked = true;
            _readingState = ReadingState.Inactive;
            _gridPos = -5;
            yield return new WaitForSeconds(2f);
            TempText.text = "-";
            _inputLocked = false;
            _isInteracting = false;
            yield break;
        }
        if (_readingState == ReadingState.AlongTheRow)
        {
            if (_input == _solutionWord)
            {
                Debug.LogFormat("[Bell of Tío #{0}] Correctly submitted \"{1}\". Module solved.", _moduleId, _input);
                _moduleSolved = true;
                Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.CorrectChime, transform);
                Module.HandlePass();
            }
            else
            {
                Debug.LogFormat("[Bell of Tío #{0}] Incorrectly submitted \"{1}\". Strike.", _moduleId, _input);
                _input = "";
                _gridPos = -5;
                Module.HandleStrike();
                TempText.text = "-";
                _inputLocked = true;
                yield return new WaitForSeconds(1.5f);
                _inputLocked = false;
                _isInteracting = false;
            }
            _readingState = ReadingState.Inactive;
        }
        yield break;
    }

    private IEnumerator SpamTimer()
    {
        yield return new WaitForSeconds(1.5f);

        if (_readingState == ReadingState.Inactive)
        {
            _readingState = ReadingState.DownTheColumn;
            if (_readingGrid != null)
                StopCoroutine(_readingGrid);
            _readingGrid = StartCoroutine(ReadGrid());
        }
        else if (_readingState == ReadingState.AlongTheRow)
        {
            var letter = _letterGrid[_gridPos];
            _input += letter;
            _inputLocked = true;
            StartCoroutine(AddLetterVoiceLine());
            Debug.LogFormat("[Bell of Tío #{0}] Current input: {1}", _moduleId, _input);
            for (int i = 0; i < 5; i++)
            {
                if (_transformations[_gridPos / 5].Contains(i))
                {
                    _letterGrid = ApplyModification(_letterGrid, _modificationsToPickFrom[i]);
                    Debug.LogFormat("[Bell of Tío #{0}] Modification applied: {1}.", _moduleId, _modificationsToPickFrom[i]);
                }
            }
            Debug.LogFormat("[Bell of Tío #{0}] The letter grid is now: {1}", _moduleId, _letterGrid);
        }
    }

    private IEnumerator AddLetterVoiceLine()
    {
        if (_input.Length == 1)
            Audio.PlaySoundAtTransform(_voice + "_First", transform);
        else
            Audio.PlaySoundAtTransform(_voice + "_Next", transform);
        TempText.text = "added";
        yield return new WaitForSeconds(1.75f);
        ReadLetter(_input.Last());
        _gridPos = -5;
        TempText.text = _input.Last().ToString();
        yield return new WaitForSeconds(1.5f);
        TempText.text = "-";
        _readingState = ReadingState.Inactive;
        _isInteracting = false;
        _inputLocked = false;
    }

    private void ReadLetter(char letter)
    {
        Audio.PlaySoundAtTransform(_voice + letter, transform);
    }

    private IEnumerator AnimateBellRinger(bool isPress)
    {
        var duration = 0.05f;
        var elapsed = 0f;
        var curYPos = BellRingerObj.transform.localPosition.z;
        var goal = isPress ? 0.115f : 0.125f;
        while (elapsed < duration)
        {
            BellRingerObj.transform.localPosition = new Vector3(0, 0, Mathf.Lerp(curYPos, goal, elapsed / duration));
            yield return null;
            elapsed += Time.deltaTime;
        }
        BellRingerObj.transform.localPosition = new Vector3(0, 0, goal);
    }

    private string ApplyModification(string str, GridModificationType modifIx)
    {
        var temp = str.ToCharArray();
        if (modifIx == GridModificationType.GridClockwise)
        {
            if (_modificationValue.IsGridClockwise)
                return new[] { str[20], str[15], str[10], str[05], str[00], str[21], str[16], str[11], str[06], str[01], str[22], str[17], str[12], str[07], str[02], str[23], str[18], str[13], str[08], str[03], str[24], str[19], str[14], str[09], str[04] }.Join("");
            else
                return new[] { str[04], str[09], str[14], str[19], str[24], str[03], str[08], str[13], str[18], str[23], str[02], str[07], str[12], str[17], str[22], str[01], str[06], str[11], str[16], str[21], str[00], str[05], str[10], str[15], str[20] }.Join("");
        }
        if (modifIx == GridModificationType.RowColumnSwap)
        {
            if (_modificationValue.RowAndColumnSwapPos == 0)
                return new[] { temp[00], temp[05], temp[10], temp[15], temp[20], temp[01], temp[06], temp[07], temp[08], temp[09], temp[02], temp[11], temp[12], temp[13], temp[14], temp[03], temp[16], temp[17], temp[18], temp[19], temp[04], temp[21], temp[22], temp[23], temp[24] }.Join("");
            if (_modificationValue.RowAndColumnSwapPos == 1)
                return new[] { temp[00], temp[05], temp[02], temp[03], temp[04], temp[01], temp[06], temp[11], temp[16], temp[21], temp[10], temp[07], temp[12], temp[13], temp[14], temp[15], temp[08], temp[17], temp[18], temp[19], temp[20], temp[09], temp[22], temp[23], temp[24] }.Join("");
            if (_modificationValue.RowAndColumnSwapPos == 2)
                return new[] { temp[00], temp[01], temp[10], temp[03], temp[04], temp[05], temp[06], temp[11], temp[08], temp[09], temp[02], temp[07], temp[12], temp[17], temp[22], temp[15], temp[16], temp[13], temp[18], temp[19], temp[20], temp[21], temp[14], temp[23], temp[24] }.Join("");
            if (_modificationValue.RowAndColumnSwapPos == 3)
                return new[] { temp[00], temp[01], temp[02], temp[15], temp[04], temp[05], temp[06], temp[07], temp[16], temp[09], temp[10], temp[11], temp[12], temp[17], temp[14], temp[03], temp[08], temp[13], temp[18], temp[23], temp[20], temp[21], temp[22], temp[19], temp[24] }.Join("");
            if (_modificationValue.RowAndColumnSwapPos == 4)
                return new[] { temp[00], temp[01], temp[02], temp[03], temp[20], temp[05], temp[06], temp[07], temp[08], temp[21], temp[10], temp[11], temp[12], temp[13], temp[22], temp[15], temp[16], temp[17], temp[18], temp[23], temp[04], temp[09], temp[14], temp[19], temp[24] }.Join("");
        }
        if (modifIx == GridModificationType.RowFlip)
        {
            if (_modificationValue.RowFlips.Contains(0))
                temp = new[] { temp[04], temp[03], temp[02], temp[01], temp[00], temp[05], temp[06], temp[07], temp[08], temp[09], temp[10], temp[11], temp[12], temp[13], temp[14], temp[15], temp[16], temp[17], temp[18], temp[19], temp[20], temp[21], temp[22], temp[23], temp[24] };
            if (_modificationValue.RowFlips.Contains(1))
                temp = new[] { temp[00], temp[01], temp[02], temp[03], temp[04], temp[09], temp[08], temp[07], temp[06], temp[05], temp[10], temp[11], temp[12], temp[13], temp[14], temp[15], temp[16], temp[17], temp[18], temp[19], temp[20], temp[21], temp[22], temp[23], temp[24] };
            if (_modificationValue.RowFlips.Contains(2))
                temp = new[] { temp[00], temp[01], temp[02], temp[03], temp[04], temp[05], temp[06], temp[07], temp[08], temp[09], temp[14], temp[13], temp[12], temp[11], temp[10], temp[15], temp[16], temp[17], temp[18], temp[19], temp[20], temp[21], temp[22], temp[23], temp[24] };
            if (_modificationValue.RowFlips.Contains(3))
                temp = new[] { temp[00], temp[01], temp[02], temp[03], temp[04], temp[05], temp[06], temp[07], temp[08], temp[09], temp[10], temp[11], temp[12], temp[13], temp[14], temp[19], temp[18], temp[17], temp[16], temp[15], temp[20], temp[21], temp[22], temp[23], temp[24] };
            if (_modificationValue.RowFlips.Contains(4))
                temp = new[] { temp[00], temp[01], temp[02], temp[03], temp[04], temp[05], temp[06], temp[07], temp[08], temp[09], temp[10], temp[11], temp[12], temp[13], temp[14], temp[15], temp[16], temp[17], temp[18], temp[19], temp[24], temp[23], temp[22], temp[21], temp[20] };
            return temp.Join("");
        }
        if (modifIx == GridModificationType.TwoColumnsSwap)
        {
            if (_modificationValue.ColumnsToSwap.Contains(0) && _modificationValue.ColumnsToSwap.Contains(1))
                return new[] { temp[01], temp[00], temp[02], temp[03], temp[04], temp[06], temp[05], temp[07], temp[08], temp[09], temp[11], temp[10], temp[12], temp[13], temp[14], temp[16], temp[15], temp[17], temp[18], temp[19], temp[21], temp[20], temp[22], temp[23], temp[24] }.Join("");
            if (_modificationValue.ColumnsToSwap.Contains(0) && _modificationValue.ColumnsToSwap.Contains(2))
                return new[] { temp[02], temp[01], temp[00], temp[03], temp[04], temp[07], temp[06], temp[05], temp[08], temp[09], temp[12], temp[11], temp[10], temp[13], temp[14], temp[17], temp[16], temp[15], temp[18], temp[19], temp[22], temp[21], temp[20], temp[23], temp[24] }.Join("");
            if (_modificationValue.ColumnsToSwap.Contains(0) && _modificationValue.ColumnsToSwap.Contains(3))
                return new[] { temp[03], temp[01], temp[02], temp[00], temp[04], temp[08], temp[06], temp[07], temp[05], temp[09], temp[13], temp[11], temp[12], temp[10], temp[14], temp[18], temp[16], temp[17], temp[15], temp[19], temp[23], temp[21], temp[22], temp[20], temp[24] }.Join("");
            if (_modificationValue.ColumnsToSwap.Contains(0) && _modificationValue.ColumnsToSwap.Contains(4))
                return new[] { temp[04], temp[01], temp[02], temp[03], temp[00], temp[09], temp[06], temp[07], temp[08], temp[05], temp[14], temp[11], temp[12], temp[13], temp[10], temp[19], temp[16], temp[17], temp[18], temp[15], temp[24], temp[21], temp[22], temp[23], temp[20], }.Join("");
            if (_modificationValue.ColumnsToSwap.Contains(1) && _modificationValue.ColumnsToSwap.Contains(2))
                return new[] { temp[00], temp[02], temp[01], temp[03], temp[04], temp[05], temp[07], temp[06], temp[08], temp[09], temp[10], temp[12], temp[11], temp[13], temp[14], temp[15], temp[17], temp[16], temp[18], temp[19], temp[20], temp[22], temp[21], temp[23], temp[24] }.Join("");
            if (_modificationValue.ColumnsToSwap.Contains(1) && _modificationValue.ColumnsToSwap.Contains(3))
                return new[] { temp[00], temp[03], temp[02], temp[01], temp[04], temp[05], temp[08], temp[07], temp[06], temp[09], temp[10], temp[13], temp[12], temp[11], temp[14], temp[15], temp[18], temp[17], temp[16], temp[19], temp[20], temp[23], temp[22], temp[21], temp[24] }.Join("");
            if (_modificationValue.ColumnsToSwap.Contains(1) && _modificationValue.ColumnsToSwap.Contains(4))
                return new[] { temp[00], temp[04], temp[02], temp[03], temp[01], temp[05], temp[09], temp[07], temp[08], temp[06], temp[10], temp[14], temp[12], temp[13], temp[11], temp[15], temp[19], temp[17], temp[18], temp[16], temp[20], temp[24], temp[22], temp[23], temp[21] }.Join("");
            if (_modificationValue.ColumnsToSwap.Contains(2) && _modificationValue.ColumnsToSwap.Contains(3))
                return new[] { temp[00], temp[01], temp[03], temp[02], temp[04], temp[05], temp[06], temp[08], temp[07], temp[09], temp[10], temp[11], temp[13], temp[12], temp[14], temp[15], temp[16], temp[18], temp[17], temp[19], temp[20], temp[21], temp[23], temp[22], temp[24] }.Join("");
            if (_modificationValue.ColumnsToSwap.Contains(2) && _modificationValue.ColumnsToSwap.Contains(4))
                return new[] { temp[00], temp[01], temp[04], temp[03], temp[02], temp[05], temp[06], temp[09], temp[08], temp[07], temp[10], temp[11], temp[14], temp[13], temp[12], temp[15], temp[16], temp[19], temp[18], temp[17], temp[20], temp[21], temp[24], temp[23], temp[22] }.Join("");
            if (_modificationValue.ColumnsToSwap.Contains(3) && _modificationValue.ColumnsToSwap.Contains(4))
                return new[] { temp[00], temp[01], temp[02], temp[04], temp[03], temp[05], temp[06], temp[07], temp[09], temp[08], temp[10], temp[11], temp[12], temp[14], temp[13], temp[15], temp[16], temp[17], temp[19], temp[18], temp[20], temp[21], temp[22], temp[24], temp[23] }.Join("");
        }
        if (modifIx == GridModificationType.RowCycle)
        {
            for (int i = 0; i < _modificationValue.RowCycleCount; i++)
            {
                if (_modificationValue.RowCycles.Contains(0))
                    temp = new[] { temp[04], temp[00], temp[01], temp[02], temp[03], temp[05], temp[06], temp[07], temp[08], temp[09], temp[10], temp[11], temp[12], temp[13], temp[14], temp[15], temp[16], temp[17], temp[18], temp[19], temp[20], temp[21], temp[22], temp[23], temp[24] };
                if (_modificationValue.RowCycles.Contains(1))
                    temp = new[] { temp[00], temp[01], temp[02], temp[03], temp[04], temp[09], temp[05], temp[06], temp[07], temp[08], temp[10], temp[11], temp[12], temp[13], temp[14], temp[15], temp[16], temp[17], temp[18], temp[19], temp[20], temp[21], temp[22], temp[23], temp[24] };
                if (_modificationValue.RowCycles.Contains(2))
                    temp = new[] { temp[00], temp[01], temp[02], temp[03], temp[04], temp[05], temp[06], temp[07], temp[08], temp[09], temp[14], temp[10], temp[11], temp[12], temp[13], temp[15], temp[16], temp[17], temp[18], temp[19], temp[20], temp[21], temp[22], temp[23], temp[24] };
                if (_modificationValue.RowCycles.Contains(3))
                    temp = new[] { temp[00], temp[01], temp[02], temp[03], temp[04], temp[05], temp[06], temp[07], temp[08], temp[09], temp[10], temp[11], temp[12], temp[13], temp[14], temp[19], temp[15], temp[16], temp[17], temp[18], temp[20], temp[21], temp[22], temp[23], temp[24] };
                if (_modificationValue.RowCycles.Contains(4))
                    temp = new[] { temp[00], temp[01], temp[02], temp[03], temp[04], temp[05], temp[06], temp[07], temp[08], temp[09], temp[10], temp[11], temp[12], temp[13], temp[14], temp[15], temp[16], temp[17], temp[18], temp[19], temp[24], temp[20], temp[21], temp[22], temp[23] };
            }
            return temp.Join("");
        }
        if (modifIx == GridModificationType.FiveRandomPositionsCycle)
        {
            var t = temp.ToArray();
            var ch = t[_modificationValue.RandomPositionsToCycle[0]];
            t[_modificationValue.RandomPositionsToCycle[0]] = t[_modificationValue.RandomPositionsToCycle[1]];
            t[_modificationValue.RandomPositionsToCycle[1]] = t[_modificationValue.RandomPositionsToCycle[2]];
            t[_modificationValue.RandomPositionsToCycle[2]] = t[_modificationValue.RandomPositionsToCycle[3]];
            t[_modificationValue.RandomPositionsToCycle[3]] = t[_modificationValue.RandomPositionsToCycle[4]];
            t[_modificationValue.RandomPositionsToCycle[4]] = ch;
            return t.Join("");
        }
        if (modifIx == GridModificationType.ColumnCycle)
        {
            for (int i = 0; i < _modificationValue.ColumnCycleCount; i++)
            {
                if (_modificationValue.ColumnCycles.Contains(0))
                    temp = new[] { temp[20], temp[01], temp[02], temp[03], temp[04], temp[00], temp[06], temp[07], temp[08], temp[09], temp[05], temp[11], temp[12], temp[13], temp[14], temp[10], temp[16], temp[17], temp[18], temp[19], temp[15], temp[21], temp[22], temp[23], temp[24] };
                if (_modificationValue.ColumnCycles.Contains(1))
                    temp = new[] { temp[00], temp[21], temp[02], temp[03], temp[04], temp[05], temp[01], temp[07], temp[08], temp[09], temp[10], temp[06], temp[12], temp[13], temp[14], temp[15], temp[11], temp[17], temp[18], temp[19], temp[20], temp[16], temp[22], temp[23], temp[24] };
                if (_modificationValue.ColumnCycles.Contains(2))
                    temp = new[] { temp[00], temp[01], temp[22], temp[03], temp[04], temp[05], temp[06], temp[02], temp[08], temp[09], temp[10], temp[11], temp[07], temp[13], temp[14], temp[15], temp[16], temp[12], temp[18], temp[19], temp[20], temp[21], temp[17], temp[23], temp[24] };
                if (_modificationValue.ColumnCycles.Contains(3))
                    temp = new[] { temp[00], temp[01], temp[02], temp[23], temp[04], temp[05], temp[06], temp[07], temp[03], temp[09], temp[10], temp[11], temp[12], temp[08], temp[14], temp[15], temp[16], temp[17], temp[13], temp[19], temp[20], temp[21], temp[22], temp[18], temp[24] };
                if (_modificationValue.ColumnCycles.Contains(4))
                    temp = new[] { temp[00], temp[01], temp[02], temp[03], temp[24], temp[05], temp[06], temp[07], temp[08], temp[04], temp[10], temp[11], temp[12], temp[13], temp[09], temp[15], temp[16], temp[17], temp[18], temp[14], temp[20], temp[21], temp[22], temp[23], temp[19] };
            }
            return temp.Join("");
        }
        if (modifIx == GridModificationType.TwoRowsSwap)
        {
            if (_modificationValue.RowsToSwap.Contains(0) && _modificationValue.RowsToSwap.Contains(1))
                return new[] { temp[05], temp[06], temp[07], temp[08], temp[09], temp[00], temp[01], temp[02], temp[03], temp[04], temp[10], temp[11], temp[12], temp[13], temp[14], temp[15], temp[16], temp[17], temp[18], temp[19], temp[20], temp[21], temp[22], temp[23], temp[24] }.Join("");
            if (_modificationValue.RowsToSwap.Contains(0) && _modificationValue.RowsToSwap.Contains(2))
                return new[] { temp[10], temp[11], temp[12], temp[13], temp[14], temp[05], temp[06], temp[07], temp[08], temp[09], temp[00], temp[01], temp[02], temp[03], temp[04], temp[15], temp[16], temp[17], temp[18], temp[19], temp[20], temp[21], temp[22], temp[23], temp[24] }.Join("");
            if (_modificationValue.RowsToSwap.Contains(0) && _modificationValue.RowsToSwap.Contains(3))
                return new[] { temp[15], temp[16], temp[17], temp[18], temp[19], temp[05], temp[06], temp[07], temp[08], temp[09], temp[10], temp[11], temp[12], temp[13], temp[14], temp[00], temp[01], temp[02], temp[03], temp[04], temp[20], temp[21], temp[22], temp[23], temp[24] }.Join("");
            if (_modificationValue.RowsToSwap.Contains(0) && _modificationValue.RowsToSwap.Contains(4))
                return new[] { temp[20], temp[21], temp[22], temp[23], temp[24], temp[05], temp[06], temp[07], temp[08], temp[09], temp[10], temp[11], temp[12], temp[13], temp[14], temp[15], temp[16], temp[17], temp[18], temp[19], temp[00], temp[01], temp[02], temp[03], temp[04] }.Join("");
            if (_modificationValue.RowsToSwap.Contains(1) && _modificationValue.RowsToSwap.Contains(2))
                return new[] { temp[00], temp[01], temp[02], temp[03], temp[04], temp[10], temp[11], temp[12], temp[13], temp[14], temp[05], temp[06], temp[07], temp[08], temp[09], temp[15], temp[16], temp[17], temp[18], temp[19], temp[20], temp[21], temp[22], temp[23], temp[24] }.Join("");
            if (_modificationValue.RowsToSwap.Contains(1) && _modificationValue.RowsToSwap.Contains(3))
                return new[] { temp[00], temp[01], temp[02], temp[03], temp[04], temp[15], temp[16], temp[17], temp[18], temp[19], temp[10], temp[11], temp[12], temp[13], temp[14], temp[05], temp[06], temp[07], temp[08], temp[09], temp[20], temp[21], temp[22], temp[23], temp[24] }.Join("");
            if (_modificationValue.RowsToSwap.Contains(1) && _modificationValue.RowsToSwap.Contains(4))
                return new[] { temp[00], temp[01], temp[02], temp[03], temp[04], temp[20], temp[21], temp[22], temp[23], temp[24], temp[10], temp[11], temp[12], temp[13], temp[14], temp[15], temp[16], temp[17], temp[18], temp[19], temp[05], temp[06], temp[07], temp[08], temp[09] }.Join("");
            if (_modificationValue.RowsToSwap.Contains(2) && _modificationValue.RowsToSwap.Contains(3))
                return new[] { temp[00], temp[01], temp[02], temp[03], temp[04], temp[05], temp[06], temp[07], temp[08], temp[09], temp[15], temp[16], temp[17], temp[18], temp[19], temp[10], temp[11], temp[12], temp[13], temp[14], temp[20], temp[21], temp[22], temp[23], temp[24] }.Join("");
            if (_modificationValue.RowsToSwap.Contains(2) && _modificationValue.RowsToSwap.Contains(4))
                return new[] { temp[00], temp[01], temp[02], temp[03], temp[04], temp[05], temp[06], temp[07], temp[08], temp[09], temp[20], temp[21], temp[22], temp[23], temp[24], temp[15], temp[16], temp[17], temp[18], temp[19], temp[10], temp[11], temp[12], temp[13], temp[14] }.Join("");
            if (_modificationValue.RowsToSwap.Contains(3) && _modificationValue.RowsToSwap.Contains(4))
                return new[] { temp[00], temp[01], temp[02], temp[03], temp[04], temp[05], temp[06], temp[07], temp[08], temp[09], temp[10], temp[11], temp[12], temp[13], temp[14], temp[20], temp[21], temp[22], temp[23], temp[24], temp[15], temp[16], temp[17], temp[18], temp[19] }.Join("");
        }
        if (modifIx == GridModificationType.ColumnFlip)
        {
            if (_modificationValue.RowFlips.Contains(0))
                temp = new[] { temp[20], temp[01], temp[02], temp[03], temp[04], temp[15], temp[06], temp[07], temp[08], temp[09], temp[10], temp[11], temp[12], temp[13], temp[14], temp[05], temp[16], temp[17], temp[18], temp[19], temp[00], temp[21], temp[22], temp[23], temp[24] };
            if (_modificationValue.RowFlips.Contains(1))
                temp = new[] { temp[00], temp[21], temp[02], temp[03], temp[04], temp[05], temp[16], temp[07], temp[08], temp[09], temp[10], temp[11], temp[12], temp[13], temp[14], temp[15], temp[06], temp[17], temp[18], temp[19], temp[20], temp[01], temp[22], temp[23], temp[24] };
            if (_modificationValue.RowFlips.Contains(2))
                temp = new[] { temp[00], temp[01], temp[22], temp[03], temp[04], temp[05], temp[06], temp[17], temp[08], temp[09], temp[10], temp[11], temp[12], temp[13], temp[14], temp[15], temp[16], temp[07], temp[18], temp[19], temp[20], temp[21], temp[02], temp[23], temp[24] };
            if (_modificationValue.RowFlips.Contains(3))
                temp = new[] { temp[00], temp[01], temp[02], temp[23], temp[04], temp[05], temp[06], temp[07], temp[18], temp[09], temp[10], temp[11], temp[12], temp[13], temp[14], temp[15], temp[16], temp[17], temp[08], temp[19], temp[20], temp[21], temp[22], temp[03], temp[24] };
            if (_modificationValue.RowFlips.Contains(4))
                temp = new[] { temp[00], temp[01], temp[02], temp[03], temp[24], temp[05], temp[06], temp[07], temp[08], temp[19], temp[10], temp[11], temp[12], temp[13], temp[14], temp[15], temp[16], temp[17], temp[18], temp[09], temp[20], temp[21], temp[22], temp[23], temp[04] };
            return temp.Join("");
        }
        if (modifIx == GridModificationType.RingCycle)
        {
            for (int i = 0; i < _modificationValue.RingCycleCount; i++)
            {
                if (_modificationValue.IsRingClockwise)
                    temp = new[] { temp[05], temp[00], temp[01], temp[02], temp[03], temp[10], temp[06], temp[07], temp[08], temp[04], temp[15], temp[11], temp[12], temp[13], temp[09], temp[20], temp[16], temp[17], temp[18], temp[14], temp[21], temp[22], temp[23], temp[24], temp[19] };
                else
                    temp = new[] { temp[01], temp[02], temp[03], temp[04], temp[09], temp[00], temp[06], temp[07], temp[08], temp[14], temp[05], temp[11], temp[12], temp[13], temp[19], temp[10], temp[16], temp[17], temp[18], temp[24], temp[15], temp[20], temp[21], temp[22], temp[23] };
            }
            return temp.Join("");
        }
        throw new InvalidOperationException("Invalid modifIx value.");
    }

#pragma warning disable 0414
    private readonly string TwitchHelpMessage = "!{0} ring 3 5 [Ring the bell to add the letter at row 3, position 5, to your input.] | !{0} ring [Ring the bell without ringing again to go off the bottom of the grid.] | !{0} ring 1 [Ring the bell and ring once to go off the right of the grid.]";
#pragma warning restore 0414

    private IEnumerator ProcessTwitchCommand(string command)
    {
        Match m;
        m = Regex.Match(command, @"^\s*ring\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        if (m.Success)
        {
            yield return null;
            while (_isInteracting)
                yield return null;
            BellSel.OnInteract(); yield return new WaitForSeconds(0.1f); BellSel.OnInteractEnded();
            yield break;
        }

        m = Regex.Match(command, @"^\s*ring\s+(?<row>[12345])\s+(?<col>[12345])\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        if (m.Success)
        {
            yield return null;
            while (_isInteracting)
                yield return null;
            BellSel.OnInteract(); yield return new WaitForSeconds(0.1f); BellSel.OnInteractEnded();
            int row = int.Parse(m.Groups["row"].Value) - 1;
            int col = int.Parse(m.Groups["col"].Value) - 1;
            while (_gridPos / 5 != row)
                yield return null;
            yield return new WaitForSeconds(0.5f);
            BellSel.OnInteract(); yield return new WaitForSeconds(0.1f); BellSel.OnInteractEnded();
            yield return new WaitForSeconds(0.15f);
            if (col == 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    BellSel.OnInteract(); yield return new WaitForSeconds(0.1f); BellSel.OnInteractEnded();
                    yield return new WaitForSeconds(0.15f);
                }
                yield break;
            }
            while (_gridPos % 5 != col)
                yield return null;
            yield return new WaitForSeconds(0.5f);
            BellSel.OnInteract(); yield return new WaitForSeconds(0.1f); BellSel.OnInteractEnded();
            yield break;
        }

        m = Regex.Match(command, @"^\s*ring\s+(?<dig>[12345])\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        if (m.Success)
        {
            yield return null;
            while (_isInteracting)
                yield return null;
            BellSel.OnInteract();
            int dig = int.Parse(m.Groups["dig"].Value) - 1;
            while (_gridPos / 5 != dig)
                yield return null;
            yield return new WaitForSeconds(0.5f);
            BellSel.OnInteract();
        }
    }

    private void TwitchHandleForcedSolve()
    {
        // In a separate function due to potential strike risk if _readingState is in AlongTheRow state with an incorrect input. If the module is left alone, it will strike.
        StartCoroutine(Autosolve());
    }

    private IEnumerator Autosolve()
    {
        // If the module's autosolve command is run while reading is going down the column
        while (_readingState == ReadingState.DownTheColumn || _inputLocked)
            yield return true;

        // If we are currently in the row, immediately ring the bell to prevent a potential strike
        if (_readingState == ReadingState.AlongTheRow)
            BellSel.OnInteract();
        while (_readingState != ReadingState.Inactive || _inputLocked)
            yield return true;

        // If there is input that needs to be reset, ring the bell and go down the column
        if (_input.Length > 0)
        {
            if (_solutionWord.StartsWith(_input))
                goto inputIsFine;
            yield return new WaitForSeconds(0.5f);
            BellSel.OnInteract(); yield return new WaitForSeconds(0.1f); BellSel.OnInteractEnded();
            while (_input.Length > 0 || _readingState != ReadingState.Inactive || _inputLocked)
                yield return true;
        }

        inputIsFine:

        // Wait until we can ring 
        yield return new WaitForSeconds(0.5f);
        while (_readingState != ReadingState.Inactive || _inputLocked)
            yield return true;

        // Add each letter to the input
        for (int i = _input.Length; i < 5; i++)
        {
            int ix = _letterGrid.IndexOf(_solutionWord[i]);
            yield return new WaitForSeconds(0.5f);
            BellSel.OnInteract(); yield return new WaitForSeconds(0.1f); BellSel.OnInteractEnded();
            while (_gridPos == -5)
                yield return null;
            while (_gridPos / 5 != ix / 5)
                yield return null;
            yield return new WaitForSeconds(0.5f);
            BellSel.OnInteract(); yield return new WaitForSeconds(0.1f); BellSel.OnInteractEnded();
            while (_gridPos % 5 != ix % 5)
                yield return null;
            yield return new WaitForSeconds(0.5f);
            BellSel.OnInteract(); yield return new WaitForSeconds(0.1f); BellSel.OnInteractEnded();
            while (_readingState != ReadingState.Inactive)
                yield return true;
        }

        // Submit the input
        yield return new WaitForSeconds(0.5f);
        BellSel.OnInteract(); yield return new WaitForSeconds(0.1f); BellSel.OnInteractEnded();
        while (_gridPos == -5)
            yield return null;
        yield return new WaitForSeconds(0.5f);
        BellSel.OnInteract(); yield return new WaitForSeconds(0.1f); BellSel.OnInteractEnded();

        while (!_moduleSolved)
            yield return true;
    }
}