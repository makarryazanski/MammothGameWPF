using MammothWPF.Models;
using System.Diagnostics;
using System.Windows.Controls;

namespace MammothWPF.Controllers
{
    /// <summary>
    /// Класс движения мамонта
    /// </summary>
    public class MammothMovement
    {
        private readonly int _mapWidth;
        private readonly int _mapHeight;
        private static readonly Random Random = new Random();
        private Models.Point _currentTarget;
        private Direction _currentDirection;

        /// <summary>
        /// Конструктор класса движения мамонта
        /// </summary>
        /// <param name="mapWidth"></param>
        /// <param name="mapHeight"></param>
        public MammothMovement(int mapWidth, int mapHeight)
        {
            _mapWidth = mapWidth;
            _mapHeight = mapHeight;
            _currentTarget = GenerateNewTarget();
            _currentDirection = GenerateNewDirection();
        }

        public Models.Point CurrentTarget => _currentTarget;

        public async Task MoveMammoth(Mammoth mammoth)
        {
            while (true)
            {
                double mammothX = Canvas.GetLeft(mammoth.Head);
                double mammothY = Canvas.GetTop(mammoth.Head);

                if (mammothX == _currentTarget.X && mammothY == _currentTarget.Y)
                {
                    Debug.WriteLine($"Мамонт достиг цели: {_currentTarget.X}, {_currentTarget.Y}");
                    _currentTarget = GenerateNewTarget();
                    _currentDirection = GenerateNewDirection();
                    Debug.WriteLine($"Новая цель мамонта: {_currentTarget.X}, {_currentTarget.Y}");
                }

                mammoth.MoveTowards(_currentTarget);
                await Task.Delay(150);
            }
        }

        /// <summary>
        /// Метод генерации новой цели в пределах карты
        /// </summary>
        /// <returns></returns>
        private Models.Point GenerateNewTarget()
        {
            return new Models.Point(Random.Next(0, _mapWidth / 20) * 20, Random.Next(0, _mapHeight / 20) * 20);
        }

        private Direction GenerateNewDirection()
        {
            var directions = Enum.GetValues(typeof(Direction));
            var direction = directions.GetValue(Random.Next(directions.Length));
            if (direction == null)
            {
                throw new InvalidOperationException("Failed to generate a new direction.");
            }
            return (Direction)direction;
        }
    }
}