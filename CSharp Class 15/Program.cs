using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharp_Class_15
{
    class Animal
    {
        public int Age { get; set; }

        public Animal(int age)
        {
            Age = age;
        }
    }

    class Test
    {
        public void Test1()
        {

        }

        public Test Test2()
        {
            return new Test();
        }
    }

    class Program
    {
        delegate int MathDelegate(int a, int b);

        delegate T GenericMathDelegate<T>(T a, T b);

        delegate bool SortDelegate<T>(T a, T b);

        delegate void MenuItem();

        static void Main(string[] args)
        {
            // Múlt órán néztük a külön projektek létrehozását, referálását és felhasználását, azaz assembly-k létrehozását
            // Hogyan tudjuk a mások által létrehozott könyvtárakat felhasználni?

            // Library (könyvtár) ~ assembly-k gyűjteménye, melyek valamilyen funkcionalitást biztosítanak

            // Háromféle módon tudjuk felhasználni
            // 1. Megkapjuk a forráskódot, amit belefordítunk a sajátunkba
            // 2. Az illető odaadja a kész könyvtárat (.dll, .exe), amit mi referálunk a saját projektünkből
            // 3. Csomagkezelő használatával

            // Csomagkezelő: eszköz, mely szoftverek/könyvtárak/akármi telepítését, frissítését, törlését segítik elő

            // Microsoft által fejlesztett csomagkezelelő .NET-hez: NuGet
            // Visual Studio-ba be van építve, de saját weboldala is van (nuget.org)

            // Hogyan érhetjük el Visual Studio-ból?
            // Solution Explorer-ben jobb klikk a solution-ön vagy a project-en, majd "Manage NuGet Packages..."
            // Itt kereshetünk és telepíthetünk csomagokat (könyvtárakat), amiket felhasználhatunk a projektünkben
            // Például: MathNet.Numerics - tudományos és mérnöki matematikai műveletek

            // Telepítéskor keletkezik egy package.config fájl az adott projekthez, ami leírja, hogy milyen csomagok lettek telepítve
            // Ez alapján a fájl alapján a Visual Studio bármikor le tudja tölteni a hiányzó csomagokat, így elég csak a fájlt mellékelni más fejlesztőknek, nem kell a letöltött csomagokat is
            // A letöltött csomagok egy packages nevű könyvtárba kerülnek a solution-ön belül

            //--------------------------------------------------

            // Nézzük meg a következő metódust:
            var a1 = string.Format("{0} {1} {2} {3}", 1, 2, 3, 4);

            // Látszólag tetszőleges számú paraméter átadhatunk
            // Hogyan?

            // Ha megnézzük a string.Format() fejlécét, akkor láthatjuk, hogy van ott egy új params kulcsszó és valójában a metódus egy tömböt vesz át
            // Ugyanezzel a megoldással mi magunk is készíthetünk ilyen metódusokat:
            AddNumbers(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);

            // Tehát a paramétereket a metódus egy tömbként kapja meg, de a metódust hívó számára tetszőleges számú paramétert lehet megadni
            // Ezt hívjuk parameter array-nek

            // Valójában egy tömböt is átadhatunk
            var array1 = new int[] {1, 2, 3, 4};
            AddNumbers(array1);

            // Üresen is hívhatjuk:
            AddNumbers();

            // Szabályok:
            // 1. Metódusonként csak 1 db parameter array lehet (különben honnan tudnánk hol kezdődik az egyik paraméter és hol a másik) (lásd: AddNumbers1 metódus)
            // 2. Ennek az 1 db-nak a paraméterlista végén kell szerepelnie (különben honnan tudnánk hol kezdődik az egyik paraméter és hol a másik) (lásd: AddNumbers2 metódus)

            // Jó:
            AddNumbers3(1, 2, 3, 4);

            //--------------------------------------------------

            // Láttuk, hogy vannak primitív (float, double stb.) és összetett (class, enum, struct) típusaink
            // Azonban van egy típus amiről eddig nem beszéltünk, mégpedig a metódusok típusa
            // A legtöbb modern nyelvben a metódusok ugyanolyan típusok, mint az összes többi, azaz úgynevezett "first class citizen"-ek

            // Mit jelent ez?
            // 1. Létrehozhatunk változót belőle
            // 2. Átadhatjuk paraméterként
            // 3. Visszatérési típusa lehet egy metódusnak
            // Stb.

            // Mindent meg lehet vele csinálni, amit megszoktunk az eddigi típusainktól

            // Hogyan definiálhatunk metódus típust?
            // A delegate kulcsszó segítségével!
            // Gondoljuk át mi határozhatja meg egy metódus típusát?
            // 1. Visszatérési típusa
            // 2. Paraméterek száma
            // 3. Paraméterek típusa
            // Ez alapján készítsük el az osztályon belül első delegate-ünket!

            // A MathDelegate minden olyan metódussal kompatibilis, amelynek int a visszatérési típusa és két int paramétert vesz át
            // Mit jelent az, hogy kompatibilis?
            // Azt, hogy egy MathDelegate típusú változó képes megfelelő visszatérési típusú és paraméterezésű metódusokat tárolni:
            MathDelegate mathDelegate1 = new MathDelegate(Add);

            // Vagy sokkal egyszerűbben:
            MathDelegate mathDelegate2 = Add;

            // var egyik esetben sem használható!!!

            // Vegyük észre, hogy nem hívtuk meg a metódust (Add()), hanem konkrétan értékül adtuk a metódust egy delegate változónak
            // Ilyenkor a változó valójában a metódus memóriacímét tárolja
            // Ezen a változón keresztül elérhetjük a metódust

            // Azaz meghívhatjuk:
            var result1 = mathDelegate2(1, 2); // 3

            // Átadhatjuk más metódusnak, aki meghívhatja:
            MathDelegateCaller(mathDelegate2, 1, 2); // 3

            // Visszatérési típusa lehet egy metódusnak:
            MathDelegate mathDelegate3 = GetMathDelegate();
            var result2 = mathDelegate3(1, 2); // 3

            // Listában tárolhatjuk őket:
            var mathDelegates = new List<MathDelegate>
            {
                Add,
                Subtract
            };

            foreach (var mathDelegate in mathDelegates)
            {
                var result3 = mathDelegate(1, 2); // Először 3, majd -1
            }

            // Tehát mindent csinálhatunk velük, amit egy hagyományos típussal

            // Megszokás szerint a delegate-eket valójában nem az osztályban, hanem a névtérben szoktuk létrehozni, hiszen ugyanúgy egy típus, de ha csak az adott osztályban használjuk, akkor jó az osztályban is
            // Természetesen delegate-eknek is lehet láthatóság módosítója, úgy ahogy egy class-nak vagy struct-nak (hiszen egy típus)

            // Még egyel tovább lépve
            // delegate-ben is használhatunk generikus típusokat:
            GenericMathDelegate<int> genericMathDelegate = Add;

            // Innentől tetszőleges típussal tudunk matematikai műveletet végrehajtani, nem csak int-tel

            // Működésük az eddig tanultaknak megfelelően zajlik!

            //--------------------------------------------------

            // Mire jó a delegate?
            // Rengeteg felhasználási módja és előnye van annak, hogy a metódusok is egyszerű típusok
            // Nézzünk meg 1-2 példát, hogy lássuk hogyan alkalmazható (NEM TELJES LISTA!!!)

            // 1. Tetszőlegs tömb rendezése:
            var numbers = new int[] {2, 1, 3, 4};

            // Nem jó:
            //SortArrayWrong(numbers);

            // A delegate megoldja, hiszen csak definiálnunk kell, hogy a szükséges típus esetén mi dönti el a sorrendet
            // A metódus meghívja a delegate segítségével átadott metódust, ami megmondja, hogy a tömb két eleme közül melyik a nagyobb
            SortArray(numbers, SortInt);

            // Innentől ugyanaz a rendező algoritmus működik tetszőleges típusra:
            var animals = new Animal[] {new Animal(5), new Animal(4), new Animal(10)};
            SortArray(animals, SortAnimal);

            // 2. Játék menü rendszer:
            Console.WriteLine("1. New Game");
            Console.WriteLine("2. Load Game");
            Console.WriteLine("3. Save Game");
            Console.WriteLine("4. Quit");

            // delegate nélkül:
            var menu = int.Parse(Console.ReadLine());

            if (menu == 1)
                NewGame();
            else if (menu == 2)
                LoadGame();
            else if (menu == 3)
                SaveGame();
            else if (menu == 4)
                Quit();

            // Vagy:
            switch (menu)
            {
                case 1:
                    NewGame();
                    break;
                case 2:
                    LoadGame();
                    break;
                case 3:
                    SaveGame();
                    break;
                case 4:
                    Quit();
                    break;
            }

            // Sokat kell gépelni/másolni és sok a hibalehetőség, illetve nehezebb bővíteni!

            // delegate:
            var menuItems = new Dictionary<int, MenuItem>
            {
                {1, NewGame},
                {2, LoadGame},
                {3, SaveGame},
                {4, Quit},
            };

            // 1 soros hívás:
            menuItems[menu]();

            // A dictionary kihelyezhető egy adattagba és sokkal olvashatóbb lesz a kód

            // Azonban az egyik leggyakoribb felhasználása a delegate-eknek az eseménykezelés!
            // Mit jelent ez?

            // Tegyük fel, hogy van egy játékosunk, akinek van élete!
            // Tegyük fel, hogy a játékosnak van bajtársa, aki ha meghal a játékos, akkor a segítségére siet!
            // Emellett a játék végét kezelő osztály is szeretné tudni, ha a játékos meghalt!
            // Sőt, az ellenfél is szeretné tudni ha a játékos meghalt, mert akkor már nem támadja!

            // Írjuk meg! (lásd: Game.cs) [csak az egyszerűség kedvéért kerülnek egy fájlba, a megfelelő megoldás az lenne ha külön lennének]

            // Mi a baj ?
            // A játékosnak mindenkiről tárolnia kell egy referenciát, ráadásul mi köze van a játékosnak ahhoz, hogy a game over mikor fut le vagy hogy az ellenfél kit támad?
            // Ráadásul ha a játékban máshol is szükség van arra az információra, hogy a játékos meghalt, akkor még bővíteni kell a játékost, aminek az lesz a vége, hogy a játékosnak van 50 referenciája és mindenkit ő hívogat!
            // További gond, hogy az OnPlayerDead-nek publikusnak kell lennie, így igazából bárki meghívhatja

            // Mi a megoldás? delegate-ek!
            // Ne a játékos szóljon külön mindenkinek! Ő csak jelezze, hogy hékás meghaltam, aztán akit érdekel az lekezeli ezt!
            // Írjuk meg! (lásd: Game.cs)

            // Láthatjuk, hogy a játékos csak jelzi, hogy meghalt és nem felelőssége, hogy kit kell meghívni!

            // Ezt hívjuk eseménykezelésnek! Jelzem, hogy engem érdekel egy esemény, majd ha az bekövetkezik, akkor értesítést kapok és lekezelem az eseményt!
            // Szokás ezt aszinkron metódushívásnak vagy mintának is nevezni
            // A metódust, amit visszahív a játékos, szokás callback-nek nevezni

            //--------------------------------------------------

            // Valójában az eseménykezelés egy annyira gyakori jelenség, hogy a C# külön rendszert vezetett be a használatára

            // Hogyan használhatjuk ezt a rendszert?
            // Hozzunk létre egy delegate-et, ami az eseményünk típusát fogja reprezentálni
            // Majd hozzunk létre egy publikus delegate típusú eseményt az event kulcsszó segítségével! (lásd: Game.cs)

            // Láthatjuk, hogy lényegében megspóroltuk a listát és a regisztráló metódust
            // Helyette csak egy darab esemény adattagunk van
            // Az eseményre a += operátor segítségével tudunk felirakozni
            // Az eseményre felirakozókat úgy tudjuk értesíteni, hogy meghívjuk az eseményt, mintha egy metódus lenne

            // Ezen kívül vannak azonban különbségek:
            // 1. Az eseményt csak az eseményt tartalmazó osztály hívhatja meg, még a leszármazottai sem (akkor sem ha protected), ezáltal biztonságos
            // 2. Az esemény nem lehet kevésbé látható mint a delegate (például a delegate private, de az esemény publikus), hiszen ha másképp lenne, akkor az eseményre felirakozók nem látnák, hogy milyen típusú az esemény
            // 3. Csak feliratkozni vagy leiratkozni lehet, felülírni nem, azaz az = operátor nem működik

            // Ezt hívjuk Publisher-Subscriber mintának

            // A háttérben az event egy sima lista, amire lényegében a +=-vel lehet feliratkozni, azaz hozzáadni magad a listához
            // Ennek megfelelően pedig a -=-vel lehet leirakozni (pl. player.PlayerDead -= OnPlayerDead;)

            // Ha az eseményre nincs feliratkozva senki és megpróbáljuk az imént látott módon meghívni az eseményt, akkor hibát kapunk (lásd: Game.cs)
            // Mi erre a megoldás?
            // 1. Ellenőrizzük, hogy az esemény null-e
            // 2. Használjuk a ?. operátort

            // A ?. működése:
            // 1. Ha a bal oldal null, akkor nem hívja meg a jobb oldalt (bárhol használható és nagyon kényelmes, ha valami lehet null)!
            Test test = null;
            test?.Test1(); // Nincs hiba

            // 2. Értékadásnál null-t ad vissza ha a hívásnak lenne visszatérési értéke!
            var result = test?.Test2(); // A result null lesz

            // 3. Láncba fűzhető
            test?.Test2()?.Test2()?.Test2()?.Test1();

            //--------------------------------------------------

            // Valójában nincs is feltétlenül szükség saját delegate létrehozására
            // A C# beépítve tartalmaz generikus delegate típusokat, amik szó szerint (majdnem) minden létező lehetőséget lefednek!

            // Action: void + tetszőleges paraméterek
            Action<int, int> actionDelegate = ActionMethod;

            // Func: tetszőleges visszatérési típus (1. generikus típus) + tetszőleges paraméterek
            Func<int, int, int> funcDelegate = FuncMethod;

            // Predicate: bool visszatérési típus + 1 darab tetszőleges paraméter
            Predicate<int> predicateDelegate = PredicateMethod;

            // Mikor használjunk saját, nevesített delegate-et?
            // Ha az értelmezéshez fontos a név vagy sokszor van használva!

            //--------------------------------------------------

            // Érdekesség:
            // delegate-ek esetén is használható a += és -=

            MathDelegate mathDelegate4 = Add;
            mathDelegate4 += Subtract;

            mathDelegate4(1, 2);

            // Mit fog csinálni?
            // Gyakorlatilag egymás után fűzi (vagy törli ha -=) az azonos típusú metódusokat
            // Ezt hívjuk multicast delegate-nek
        }

        static int AddNumbers(params int[] parameters)
        {
            return parameters.Sum();
        }

        // Nem jó
        /*static int AddNumbers1(params int[] parameters1, params string[] parameters2)
        {

        }*/

        // Nem jó
        /*static int AddNumbers2(params int[] parameters, int a)
        {

        }*/

        // Jó
        static int AddNumbers3(int a, params int[] parameters)
        {
            return a + parameters.Sum();
        }

        static int Add(int a, int b)
        {
            return a + b;
        }

        static int Subtract(int a, int b)
        {
            return a - b;
        }

        static void MathDelegateCaller(MathDelegate mathDelegate, int a, int b)
        {
            Console.WriteLine(mathDelegate(a, b));
        }

        static MathDelegate GetMathDelegate()
        {
            return Add;
        }

        // Nem jó, mert két T-t nem tudunk összehasonlítani, hiszen nem tudjuk milyen típus
        /*static void SortArrayWrong<T>(T[] array)
        {
            for (int i = 0; i < array.Length - 1; i++)
            {
                for (int j = i + 1; j < array.Length; j++)
                {
                    if (array[j] < array[i])
                    {
                        var temp = array[i];
                        array[i] = array[j];
                        array[j] = temp;
                    }
                }
            }
        }*/

        static void SortArray<T>(T[] array, SortDelegate<T> sortDelegate)
        {
            for (int i = 0; i < array.Length - 1; i++)
            {
                for (int j = i + 1; j < array.Length; j++)
                {
                    if (sortDelegate(array[i], array[j]))
                    {
                        var temp = array[i];
                        array[i] = array[j];
                        array[j] = temp;
                    }
                }
            }
        }

        static bool SortInt(int a, int b)
        {
            return a < b;
        }

        static bool SortAnimal(Animal a, Animal b)
        {
            return a.Age < b.Age;
        }

        static void NewGame()
        {

        }

        static void LoadGame()
        {

        }

        static void SaveGame()
        {

        }

        static void Quit()
        {

        }

        static void ActionMethod(int a, int b)
        {

        }

        static int FuncMethod(int a, int b)
        {
            return a + b;
        }

        static bool PredicateMethod(int a)
        {
            return a > 0;
        }
    }
}
