namespace Swlh.WebApp.Application.Dtos.NaverVikoDtos;

public class WordItemType
{
    public string? Rank { get; set; }
    public string? Gdid { get; set; }
    public string? SourceCid { get; set; }
    public string? dictId { get; set; }
    public string? matchType { get; set; }
    public string? entryId { get; set; }
    public string? serviceCode { get; set; }
    public string? languageCode { get; set; }
    public string? expDictTypeForm { get; set; }
    public string? dictTypeForm { get; set; }
    public string? sourceDictnameKO { get; set; }
    public string? sourceDictnameOri { get; set; }
    public string? sourceDictnameLink { get; set; }
    // sourceUpdate
    // dictTypeWriter
    // dictTypeMulti
    public string? expEntry { get; set; }
    public string? expEntrySuperscript { get; set; }
    public string? destinationLink { get; set; }
    public string? destinationLinkKo { get; set; }
    // expAliasEntryAlways
    // expAliasEntryAlwaysOld
    // expAliasEntrySearch
    // expAliasEntrySearchKrKind
    // expAliasEntrySearchAllKind
    // expAliasGeneralAlways
    // expAliasGeneralSearch
    // expConjugationMoreURL
    // conjugate
    public string? expSynonym { get; set; }
    public string? expAntonym { get; set; }
    public int? pronunFileCount { get; set; }
    public int? priority { get; set; }
    public string? expKanji { get; set; }
    public string? expAudioRead { get; set; }
    public string? expMeaningRead { get; set; }
    public string? expKoreanPron { get; set; }
    public string? expKoreanHanja { get; set; }
    public string? exphanjaStroke { get; set; }
    public string? exphanjaRadical { get; set; }
    public string? exphanjaRadicalStroke { get; set; }
    public string? partGroupYn { get; set; }
    public string? newEntry { get; set; }
    public int? isHighDfTerm { get; set; }
    public int? isOpenDict { get; set; }
    public int? isPhoneticSymbol { get; set; }
    public int? hasConjugation { get; set; }
    public int? hasIdiom { get; set; }
    public int? hasExample { get; set; }
    public int? hasStudy { get; set; }
    public int? hasImage { get; set; }
    public int? hasSource { get; set; }
    public int? hasOrigin { get; set; }
    public int? meaningCount { get; set; }
    // entryImageURL
    // entryImageURLs
    public string? idiomOri { get; set; }
    public string? idiomOriUrl { get; set; }
    // phoneticSymbol
    // frequencyAdd
    // entryLikeNumber
    // entryCommentNumber
    // uuid
    public double? documentQuality { get; set; }
    // expHanjaRadicalKoreanName
    // etcExplain
    // expSourceBook
    // expEntryComposition
    // expStrokeAnimation
    // expAbstract
    // imageFileCount
    // audioThumnail
    // audioFileCount
    // videoThumnail
    // videoFileCount
    public string? expOnly { get; set; }
    // pageView

    public List<MeansCollectorType>? meansCollector { get; set; }
    //public List<>? similarWordList { get; set; }
    //public List<>? antonymWordList { get; set; }
    //public List<>? expAliasEntryAlwaysList { get; set; }
    //public List<>? expAliasGeneralAlwaysList { get; set; }
    //public List<>? expAliasEntrySearchList { get; set; }
    public List<SearchPhoneticSymbolListType>? searchPhoneticSymbolList { get; set; }
    public AbstractContent? AbstractContent { get; set; }
    //public List<>? expEntryCustomContentList { get; set; }
    //public List<>? expMeaningCustomContentList { get; set; }

    public bool? exactMatch { get; set; }
    public bool? openExactMatch { get; set; }
    public string? handleEntry { get; set; }
    public string? vcode { get; set; }
    public string? encode { get; set; }
}
