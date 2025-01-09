module snake.f.console.Renderer

open snake.f.console.GameTypes

let renderMap (map : (GameObject * Direction)[,], score : int) =
    let width = Array2D.length2 map
    let height = Array2D.length1 map

    printfn "%s" (String.replicate (width + 2) "#")

    for x = 0 to height - 1 do
        printf "#"
        for y = 0 to width - 1 do
            let obj, _ = map.[x, y]
            match obj with
            | GameObject.Snake -> printf "O"
            | GameObject.Food -> printf "*"
            | GameObject.Ground -> printf " "
        printf "#\n"

    printfn "%s" (String.replicate (width + 2) "#")

    printfn $"Score: %d{score}"
