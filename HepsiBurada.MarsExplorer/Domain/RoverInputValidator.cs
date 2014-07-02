﻿namespace Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class RoverInputValidator : IRoverBuilder
    {
        private readonly IRoverBuilder _roverBuilder;

        public RoverInputValidator(IRoverBuilder roverBuilder)
        {
            _roverBuilder = roverBuilder;
        }

        public Rover Build(string coordinate, string movement, Plateu plateu)
        {
            ValidateCoordinate(coordinate);
            ValidateMovement(movement);
            return _roverBuilder.Build(coordinate, movement, plateu);
        }

        private void ValidateMovement(string command)
        {
            if (string.IsNullOrWhiteSpace(command))
            {
                throw new ArgumentException("Please enter a movement command which is not empty or whitespace");
            }

            if (!command.All(
                x =>
                {
                    MovementType output;
                    return Enum.TryParse(x.ToString(), out output);

                }))
            {
                throw new ArgumentException("Please enter a valid movement command. Movement command must contain only M,L,R charahters.");
            }
        }

        private void ValidateCoordinate(string command)
        {
            if (string.IsNullOrWhiteSpace(command))
            {
                throw new ArgumentException("Please enter a coordinate which is not empty or whitespace");
            }
            var coordinates = SplitCoordinate(command).ToList();
            if (coordinates.Count() != 3)
            {
                throw new ArgumentException(
                    "Invalid number of coordinate parameter. Please provide coordinates and direction of the rover. Like '1 1 N'.");
            }

            foreach (var coordinate in coordinates.Take(2))
            {
                int result;
                if (!int.TryParse(coordinate, out result))
                {
                    throw new ArgumentException(
                        string.Format("Please provide a valid position for coordinate. {0} is not a valid number.", coordinate));
                }

                if (result < 0)
                {
                    throw new ArgumentException(
                        string.Format("Please provide a valid position for coordinate. {0} is not a positive number.", coordinate));
                }
            }

            var direction = coordinates.Last();
            Compass enumResult;
            if (!Enum.TryParse(direction, out enumResult))
            {
                throw new ArgumentException(
                        string.Format("Please provide a valid direction for coordinate. {0} is not a valid direction.", direction));
            }
        }

        private IEnumerable<string> SplitCoordinate(string command)
        {
            return command.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}