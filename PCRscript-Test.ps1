$path = "H:\Powershell\Quarterly"
$YearMonth = 201903
$shortNameCollection = @( "minum2",	"johnr2",	"mcnass",	"amerba",	"tickin",	"surbow",	"ikedas",	"womfou",	"catcem",	"hamamo",	"maryms",	"famkan",	"colven",	"sanchz",	"rftrus",	"millep",	"borich",	"catifa",	"oriste",	"divine",	"methlo",	"fandre",	"leewis",	"galcob",	"lothro",	"sanlon",	"gthree",	"hlavka",	"larint",	"cooand",	"olligp",	"towfam",	"arngum",	"rthorp",	"lynmer",	"cemcat",	"schlwi",	"towhel",	"jesson",	"egreen",	"heddon",	"cadleg",	"johgui",	"mckenk",	"armder",	"baronf",	"thedon",	"susbro",	"IF000569",	"davru1",	"moumba",	"rosira",	"jesste",	"kurwol",	"dainor",	"edwapa",	"ayajor",	"bluecb",	"linber",	"pealea",	"laksny",	"KUMFD2",	"hecker",	"bellmi",	"ameant",	"sanabh",	"bolbry",	"gongen",	"bemarl",	"sanber",	"poulab",	"misinv",	"dowkel",	"carkel",	"dalsus",	"strede",	"landef",	"katsch",	"dean99",	"luangr",	"tabbwi",	"brideb",	"tatefa",	"peardj",	"rabina",	"hufhuf",	"coogen",	"ankepa",	"smsllc",	"oremus",	"jusowa",	"micint",	"ddweek",	"gpwdom",	"seajoh",	"gumshe",	"mranic",	"zacgre",	"jtihld",	"thomj2",	"bryche",	"thorgm",	"spears",	"braark",	"partor",	"mcnitt",	"ctuced",	"giljul",	"jamind",	"sikrhe",	"majoah",	"jocrow",	"josreg",	"hippoc",	"hiloct",	"hynjos",	"kirrag",	"ritdiv",	"robvet",	"stegin",	"johant",	"greyco",	"romarl",	"dontte",	"beerwi",	"deming",	"robbec",	"johill",	"blainr",	"kspnho",	"kriegg",	"aforth",	"ericjh",	"giordo",	"famfou",	"adbfam",	"decfou",	"consco",	"tarbro",	"schakc",	"jamben",	"jlbucc",	"mellse",	"mayadm",	"mcgelt",	"chiatr",	"zcaplc",	"ricald",	"mascla",	"gilinv",	"gillia",	"relitr",	"hexaph",	"prisel",	"beganl",	"milhal",	"IF142544",	"alldal",	"jkshaw",	"blaste",	"ecclma",	"llcjhs",	"chabeu",	"ethdav",	"deladl",	"smilin",	"mibitc",	"begfam",	"urrure",	"IF142782",	"linsto",	"rosloa",	"pdruck",	"lardom",	"mullga",	"gartra",	"gragre",	"harcou",	"sharma",	"malmir",	"jeffer",	"hayd10",	"rooger",	"browwc",	"wadgoe",	"jsdoke",	"lawryc",	"hamber",	"ctuctr",	"julhar",	"jefgbl",	"sparis",	"nicole",	"gbrown",	"alaskh",	"frefre",	"mahshe",	"goldgb",	"meehho",	"manirm",	"janepm",	"IF142530",	"mabeaz",	"lwndin",	"babhen",	"tramha",	"phigar",	"amorcr",	"schinm",	"chaang",	"garyou",	"vonhu1",	"gwidaj",	"marbea",	"ganalo",	"rubwad",	"millmat",	"frihar",	"meiven",	"ragajk",	"packin",	"colcar",	"mezzkar",	"greben",	"rafdat",	"knnedy",	"barert",	"basker",	"danert",	"nazsco",	"kramf5",	"radval",	"felddo",	"dament",	"nodewa",	"A90862",	"bretdw",	"riwald",	"ivpart",	"bellit",	"pichai",	"sjandi",	"molcar",	"direel",	"hagetr",	"burgfr",	"casnic",	"robtee",	"grintl",	"brubla",	"kariag",	"jaclev",	"rutzmi",	"ngaren",	"lamger",	"texcru",	"chaflo",	"geoaut",	"sethia",	"morset",	"martna",	"wtrees",	"estrju",	"yolles",	"janwar",	"tunain",	"jrritr",	"salmun",	"taneda",	"mottoh",	"cofvin",	"plabar",	"grefat",	"rofaeq",	"famhic",	"inrust",	"sinctr",	"lunott",	"micall",	"ettent",	"tertho",	"dargc4",	"jambor",	"suzbro",	"adeold",	"chufam",	"hawort",	"aberry",	"allkin",	"avisfam",	"bangtail",	"owskev",	"harige",	"gcmj02",	"burlma",	"defrei",	"helmcg",	"racmcg",	"greott",	"carlle",	"jorgan",	"clajaq",	"keicox",	"fredcr",	"medsub",	"D28634",	"dannin",	"lucasa",	"deanna",	"deedub",	"derang",	"litflo",	"kennyo",	"brankd",	"donjor",	"drumtw",	"davrea",	"edtluc",	"kilves",	"colfnd",	"lesli2",	"chigie",	"moorintl",	"fergsa",	"finejo",	"IF142812",	"fradow",	"gauthie2",	"ghemsa",	"gjohns",	"gladia",	"davive",	"goldga",	"gorete",	"gradf1",	"hestia",	"hrgtru",	"iamjoh",	"IF142940",	"IF142528",	"IP001133",	"abraal",	"lowper",	"pjactr",	"jonbre",	"edtoni",	"karred",	"horowitz",	"homkri",	"sallfa",	"krumpp",	"itikum",	"lacola",	"loiane",	"thamaa",	"hawkle",	"alirog",	"cliacw",	"margui",	"mezzda2",	"paslay",	"avaem2",	"valev1",	"delfin",	"KIRBFINT",	"sullid",	"robsar",	"masker",	"perse2",	"hoagfam",	"rose46",	"carneg",	"nicst1",	"grande",	"pateam",	"bowman",	"lpwfdn",	"minica",	"putfam",	"widintl",	"stille",	"pedrir",	"pigoth",	"police",	"gisgam",	"ginsin",	"katche",	"sladey",	"dasant",	"allgre",	"mnbiru",	"blanna",	"whijos",	"prasra",	"premri",	"progra",	"proved",	"people",	"widatu",	"robejj",	"rafcor",	"maryre",	"joneje",	"sentru",	"livlor",	"majtst",	"famigu",	"maumar",	"raychu",	"reesja",	"reindom",	"reinintl",	"lesfam",	"hirstj",	"coymar",	"kvamef",	"AMFWPA",	"hartra",	"hagtho",	"hawllc",	"truluc",	"veraub",	"strykr",	"zarkan",	"smthtr",	"dklpar",	"phigeo",	"chigbe",	"stikru",	"dennig",	"stefis",	"jenmos",	"richmr",	"agltru",	"ihsru3",	"barkra",	"butnce",	"saddca",	"bootly",	"woodsml",	"higgtr",	"salila",	"sanch2",	"ancgbl",	"ohiosb",	"schatz",	"zinggp",	"freefm",	"toback",	"shepal",	"shook",	"famsrk",	"simric",	"steink",	"jayhud",	"lbroth",	"pollse",	"dndwil",	"kabrob",	"robkof",	"rafind",	"H72394",	"heckje",	"doldou",	"goodval",	"harr97",	"moretr",	"culctu",	"weisjo",	"commca",	"gleibf",	"woodhl",	"longdi",	"badrma",	"librel",	"Y73389",	"ricsca",	"jgmsp6",	"scoirr",	"octllc",	"rosegl",	"margdj",	"booot2",	"stamm",	"chisul",	"wamark",	"uniia1",	"kattru",	"fisspv",	"hanjon",	"eslaug",	"kengee",	"vers70",	"vers60",	"virden",	"volkst",	"mfcace",	"foxlan",	"roweil",	"multintl",	"wilkor",	"markm1",	"chungk",	"kvamme",	"hulfam",	"richr1" )


$Taxable = 'true'
$Id = 442946
$Shortname = '122llc'
$Url = "http://sea-500-13.paraport.com/ReportServer/Pages/ReportViewer.aspx?/PPA.PCR.Reports/Quarterly-Formal&Year_Month=$YearMonth&id=$Id&taxable=$Taxable&rs:ClearSession=true&rs:Format=PDF&rc:DocMap=false&rs:Command=Render"


$SqlConnection = New-Object System.Data.SqlClient.SqlConnection
$SqlConnection.ConnectionString = "Server=uniondb.dev.paraport.com;Database=APP_SUPPORT_PCR;Integrated Security=True"
$SqlCmd = New-Object System.Data.SqlClient.SqlCommand
$SqlParameter = New-Object System.Data.SqlClient.SqlParameter
$SqlParameter.ParameterName = "@year_month"
$SqlParameter.DbType = [System.Data.DbType]::Int32
$SqlParameter.Value = $YearMonth
$SqlCmd.CommandType = [System.Data.CommandType]::StoredProcedure
$SqlCmd.CommandText = "sp_List_QTRLY"
$SqlCmd.Connection = $SqlConnection

$Sqlcmd.Parameters.Add($SqlParameter)

$SqlAdapter = New-Object System.Data.SqlClient.SqlDataAdapter
$SqlAdapter.SelectCommand = $SqlCmd
$DataSet = New-Object System.Data.DataSet
$SqlAdapter.Fill($DataSet)
$SqlConnection.Close()


foreach ($Row in $DataSet.Tables[0].Rows)
{ 
    $Id = $($Row["id"])
    $Shortname = $($Row["account_short_name"]).ToLower()
    $tValue = $($Row["taxable"])

    if ($tValue -eq 1) 
    {
        $Taxable = "true"
    }
    else
    {
        $Taxable = "false"
    }

    $Url = "http://sea-500-13.paraport.com/ReportServer/Pages/ReportViewer.aspx?/PPA.PCR.Reports/Quarterly-Formal&Year_Month=$YearMonth&id=$Id&taxable=$Taxable&rs:ClearSession=true&rs:Format=PDF&rc:DocMap=false&rs:Command=Render"
    $SecondUrl = "http://sea-500-13.paraport.com/ReportServer/Pages/ReportViewer.aspx?/PPA.PCR.Reports/Quarterly-Formal&Year_Month=$YearMonth&id=$Id&taxable=$Taxable&rs:ClearSession=true&rs:Format=XLS&rc:DocMap=false&rs:Command=Render"

    if ($shortNameCollection -contains $Shortname)
    {
        if(!(Test-Path -Path "$path\$YearMonth-$ShortName.pdf"))
        {
            Write-Host "Writing '$shortname'"
            Invoke-WebRequest -Uri $Url -UseDefaultCredentials -OutFile "$path\$YearMonth-$ShortName.pdf"
            Invoke-WebRequest -Uri $SecondUrl -UseDefaultCredentials -OutFile "$path\$YearMonth-$ShortName.xls"
        }
    }

}