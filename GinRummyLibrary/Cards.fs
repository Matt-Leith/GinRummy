module Cards

type Suit = Spades | Clubs | Hearts | Diamonds
type Rank = Ace | Two | Three | Four | Five | Six | Seven | Eight | Nine | Ten | Jack | Queen | King
type Card = { suit: Suit; rank: Rank}

type Hand = Card seq
type Deck = Card seq

let AllSuits = [ Spades; Clubs; Hearts; Diamonds ]
let AllRanks = [ Ace; Two; Three; Four; Five; Six; Seven; Eight; Nine; Ten; Jack; Queen; King ]

let returnTuple tup = 
    let (x, y) = tup
    y

// Add helper functions to help compute Deadwood function
let calcSets (hand:Hand) =  
    let ranks = 
        seq { 
            for card in hand do
            yield card.rank
        }           
        
    let result = ranks |> Seq.countBy id |> Seq.toList
    let answer = 
        seq {
            for each in result do
                yield returnTuple each
            }
    answer

let randCard = {suit=Clubs; rank=Ace}
let randCard2 = {suit=Hearts; rank=Ace}
let hand1 = seq {yield randCard}
let hand2 = Seq.append hand1 [{suit=Hearts; rank=Ace}]

let lol = calcSets hand2
lol


let rec calcCardVal (rank:Rank) count = 
    if rank <> AllRanks.Item(10) && rank <> AllRanks.Item(11) && rank <> AllRanks.Item(12) then        
        if rank = AllRanks.Item(count) then
            let count = count + 1  
            count
        else
            let count = count + 1  
            calcCardVal rank count 
    else
        10

let allCards = 
    seq { 
        for s in AllSuits do
            for r in AllRanks do
                yield {suit=s; rank=r}
    }

let FullDeck = 
    allCards


let Shuffle (deck:Deck) = 
    let random = new System.Random()
    deck |> Seq.sortBy (fun card -> random.Next())

    // Add other functions here related to Card Games ...