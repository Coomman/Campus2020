namespace thegame.Models
{
    public class UserInputForMovesPost
    {
        public UserInputForMovesPost(char keyPressed, Vec clickedPos)
        {
            KeyPressed = keyPressed;
            ClickedPos = clickedPos;
        }

        public readonly char KeyPressed;
        public readonly Vec ClickedPos;
    }
}