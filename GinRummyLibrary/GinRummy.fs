module GinRummy

open Cards

// Add helper functions to help compute Deadwood function
let returnTuple tup = 
    let (x, y) = tup
    let result = 0
    if y = 3 then
        let result = (calcCardVal x 0) * -3
        result
    else if y = 4 then
        let result = (calcCardVal x 0) * -4
        result
    else 
        result

//pass a hand
let inSet (hand:Hand) (handCard:Card) = 
    let ranks = 
        seq { 
            for card in hand do
            if card = handCard then
                yield card.rank
        }
        
    let result = ranks |> Seq.length  
    
    if result >= 3 then
        true
    else
        false        

let rec getHighestNonSetCard (hand:Hand) highestCard count = 
    //let highestCard = [];
    let handList = hand |> Seq.toList
    let card = handList.Item(count)
    //for each in hand do 
    if not (inSet hand card) then
        if calcCardVal card.rank > hghestCard then
            let highestCard = calcCardVal card.rank
        let highestCard = calcCardVal card.rank
        let count = count + 1
        getHighestNonSetCard hand highestCard count
    else if count >= 9 then
        //let count = count + 1
        highestCard
    else 
        let count = count + 1
        getHighestNonSetCard hand highestCard count
    

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
    let answer = answer |> Seq.sum
    answer

let Deadwood (hand:Hand) = 
    let setOffset = calcSets hand   

    let handScores = 
        seq { 
            for card in hand do
            yield calcCardVal card.rank 0
        }

    let score = handScores |> Seq.sum
    let score = score + setOffset
    score

let Score (firstOut:Hand) (secondOut:Hand) =
    0
    // Fixme change so that it computes how many points should be scored by the firstOut hand
    // (score should be negative if the secondOut hand is the winner)

// Add other functions related to Gin Rummy here ...