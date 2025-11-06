using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace LAB5
{
    /// <summary>
    /// Перечисление, хранящее в себе уникальные индефикаторы (IDs) для меню в главном методе.
    /// </summary>
    public enum ActionTypes
    {
        TryToGuess = 1,
        Author = 2,
        Sorting = 3,
        Game = 4,
        Exit = 5
    }

    static class Program
    {
        static ActionTypes Menu()
        {
            string text = "1. Отгадай ответ\n" +
                "2. Об авторе\n" +
                "3. Сортировка массивов\n" +
                "4. Игра\n" +
                "5. Выход\n" +
                "Введите действие: ";
            return (ActionTypes)RequestHelper.RequestInt(text, value => Enum.IsDefined<ActionTypes>((ActionTypes)value));
        }
        static void AboutAuthor()
        {
            Console.WriteLine("Работу выполнил Галацков Артем из группы 6106-090301D");
        }

        static void Main(string[] args)
        {
            bool isProgramWorking = true;
            SnakeGame game = new SnakeGame(200);
            ArrayHelper arrayHelper;
            while (isProgramWorking)
            {
                ActionTypes action = Menu();
                switch (action)
                {
                    case ActionTypes.TryToGuess:
                        ToGuessGame.Play();
                        break;
                    case ActionTypes.Exit:
                        isProgramWorking = !RequestHelper.RequestBool("Вы уверены? (д/н): ", "д", "н");
                        break;
                    case ActionTypes.Sorting:
                        arrayHelper = new ArrayHelper(RequestHelper.RequestInt("Введите длину массива: ", value => value > 0));
                        arrayHelper.ArrayMenu();
                        break;
                    case ActionTypes.Game:
                        game.StartGame();
                        game.Reset();
                        break;
                    case ActionTypes.Author:
                        AboutAuthor();
                        break;
                }
            }
        }
    }
}