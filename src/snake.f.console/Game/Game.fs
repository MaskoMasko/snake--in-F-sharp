module snake.f.console.Game

open snake.f.console.GameTypes

let rec findTail(map : (GameObject * Direction)[,],(x, y) : int * int) =
    let _, dir = map.[x, y]
    let newX, newY =
        match dir with
        | Direction.Up -> (x + 1, y)
        | Direction.Down -> (x - 1, y)
        | Direction.Left -> (x, y + 1)
        | Direction.Right -> (x, y - 1)
        | _ -> (x, y)
    
    let obj, _ = map.[newX, newY]
    if obj = GameObject.Snake then
        findTail(map, (newX, newY))
    else
        (x, y)

let rec placeFood (map : (GameObject * Direction)[,]) =
    let rnd = System.Random()
    let x = rnd.Next(0, 18)
    let y = rnd.Next(0, 18)
    let obj, _ = map.[x, y]
    if obj = GameObject.Snake then
        placeFood(map)
    else
        (x, y)

let createBoard =
    let map = Array2D.init 18 18 (fun _ _ -> (GameObject.Ground, Direction.None))
    map.[8, 8] <- (GameObject.Snake, Direction.Up)
    let foodX, foodY = placeFood(map)
    map.[foodX, foodY] <- (GameObject.Food, Direction.None)
    { Map = map
      Direction = Direction.Up
      Position = (8, 8)
      Score = 0
      Dead = false }
    
let modifyHitFood (map : (GameObject * Direction)[,],(x, y) : int * int,direction : Direction) =
    map.[x, y] <- (GameObject.Snake, direction)
    let foodX, foodY = placeFood(map)
    map.[foodX, foodY] <- (GameObject.Food, Direction.None)
    map

let modifyHitGround (map : (GameObject * Direction)[,], (x, y) : int * int, direction : Direction) =
    map.[x, y] <- (GameObject.Snake, direction)
    let tailX, tailY = findTail(map, (x, y))
    map.[tailX, tailY] <- (GameObject.Ground, Direction.None)
    map

let gameLoop (gameContext : GameContext) =
    let x, y = gameContext.Position
    let newX, newY =
        match gameContext.Direction with
        | Direction.Up -> (x - 1, y)
        | Direction.Down -> (x + 1, y)
        | Direction.Left -> (x, y - 1)
        | Direction.Right -> (x, y + 1)
        | _ -> (x, y)
    
    try
        let obj, _ = gameContext.Map.[newX, newY]
        match obj with
        | GameObject.Snake -> { gameContext with
                                    Dead = true
                                    Position = (newX, newY)
                                }
        | GameObject.Food -> { gameContext with
                                    Map = modifyHitFood(gameContext.Map, (newX, newY), gameContext.Direction)
                                    Position = (newX, newY)
                                    Score = gameContext.Score + 10
                                }
        | GameObject.Ground -> { gameContext with
                                    Map = modifyHitGround(gameContext.Map, (newX, newY), gameContext.Direction)
                                    Position = (newX, newY)
                                }
    with
    | :? System.IndexOutOfRangeException -> { gameContext with Dead = true }
    
