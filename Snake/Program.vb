Imports System

'NOTES
'--Effeciency of movement to be improved
'--Possibility to lose to be added
'--Snake to elongate when eating fruits

'fps: 10 = 100ms

Module Program
    Sub instanceError()
        Console.Beep() 'change 
    End Sub
    Sub checkCollision()
        If snake.xPos = fruit.xPos And snake.yPos = fruit.yPos Then
            Dim unused = CoordList.Remove(fruit)
            generateNewFruit()
            addPoint()
            increaseLength()
        End If
    End Sub
    'timer subroutine
    Public Sub WaitForKeyInConsole(Optional MaxMilliseconds As Long = 10000)
        Dim stpw As Stopwatch = Stopwatch.StartNew
        Console.ForegroundColor = Console.ForegroundColor.Green
        Console.Write("Time left: ")
        Dim x As Long = Console.CursorLeft
        Dim y As Long = Console.CursorTop

        Do
            Threading.Thread.Sleep(10) ' non-blocking; watch cpu usage
            Console.SetCursorPosition(x, y)
            Console.ForegroundColor = Console.ForegroundColor.Green
            Console.Write((MaxMilliseconds - stpw.ElapsedMilliseconds) / 1000)

            'keep ticking down timer until:
        Loop Until Console.KeyAvailable Or stpw.ElapsedMilliseconds >= MaxMilliseconds

    End Sub
    Private Class Coord
        Public xPos As Integer
        Public yPos As Integer
        Public RedrawChar As Char
    End Class

    Dim direction As Integer
    '1,2,3,4 = left,right,up,down

    Dim CoordList As New List(Of Coord)
    Dim snake As New Coord
    Dim fruit As New Coord
    Dim points As Integer = 0
    Dim length As Integer = 0
    Dim speed As Integer = 75

    Sub increaseLength()
        length += 1

    End Sub

    'sub that manages points 
    Sub addPoint()
        points += 1
        speed -= 5
        Console.SetCursorPosition(7, 0) ' move the cursor slightly after the text b4 writing
        Console.Write(points)
    End Sub
    Sub generateNewFruit()
        Randomize()
        fruit.xPos = Int(Int((100 * Rnd()) + 1))
        fruit.yPos = Int(Int((25 * Rnd()) + 1))

        If fruit.xPos Mod 2 <> 0 Then 'x position needs to be a multiple of 2 to be reachable.
            fruit.xPos += 1 'make even
        End If

        fruit.RedrawChar = "O"
        CoordList.Add(fruit)
    End Sub
    Sub startUp()
        generateNewFruit()

        Console.Title = "Snake Game"
        Console.ForegroundColor = Console.ForegroundColor.Green
        Console.WriteLine("Points: ") 'make the points thing
        snake.RedrawChar = "■" 'must be a character

        snake.xPos = 56 ' centre the snake
        snake.yPos = 14

        'add object(s) for rendering
        CoordList.Add(snake)
        Console.CursorVisible = False 'hide the cursor
    End Sub
    'output the table 
    Sub render()


        'delete previous square
        Console.SetCursorPosition(snake.xPos, snake.yPos)
        Console.WriteLine(" ")

        checkCollision()

        Select Case direction
            'we would need to increase the x value more to keep movement consistent.
            Case = 1
                snake.xPos -= 2
            Case = 2
                snake.xPos += 2
            Case = 3
                snake.yPos += 1
            Case = 4
                snake.yPos -= 1
        End Select

        For i = 0 To CoordList.Count - 1
            Dim e As Coord = CoordList(i)

            'make 'fruits' red and the rest green
            If e Is fruit Then
                Console.ForegroundColor = Console.ForegroundColor.Red
            Else
                Console.ForegroundColor = Console.ForegroundColor.Green
            End If

            'draw the character onto the coord
            Console.SetCursorPosition(e.xPos, e.yPos)
            Console.Write(e.RedrawChar)

            'move the cursor offscreen (if visible)
            Console.ResetColor()
        Next

    End Sub
    Sub Main(args As String())
        startUp()
        Dim inputChar As ConsoleKeyInfo
        Do
            If Console.KeyAvailable Then
                inputChar = Console.ReadKey(True) 'set to true to hide keypresses
            End If

            Select Case inputChar.Key
                Case ConsoleKey.D
                    direction = 2
                Case ConsoleKey.A
                    direction = 1
                Case ConsoleKey.W
                    direction = 4
                Case ConsoleKey.S
                    direction = 3
            End Select

            'lower for faster response time, however watch cpu usage 
            '100mms = 10 fps

            Threading.Thread.Sleep(speed)
            'render image
            render()
        Loop
    End Sub
End Module