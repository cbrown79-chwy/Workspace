let seqOfThings x = seq { for n in 0 .. x do yield n * n + 3 } 

// Sequences are dynamically loaded, rather that loaded all at once.

let howLongWouldYouExpectThisToBe = seqOfThings 2000 


Seq.take 15 howLongWouldYouExpectThisToBe

// notice it only shows you the first few.

Seq.takeWhile (fun n -> n < 10) howLongWouldYouExpectThisToBe

