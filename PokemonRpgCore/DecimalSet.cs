using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonRpg
{
    public class DecimalSet
    {
        private bool _isSorted;
        private List<float> _numbers;

        public DecimalSet()
        {
            _numbers = new List<float>();
        }

        public void AddNumber(float num)
        {
            _numbers.Add(num);
            _isSorted = false;
        }

        public void AddSet(DecimalSet other)
        {
            _numbers.AddRange(other._numbers);
            _isSorted = false;
        }

        public int Count
        {
            get
            {
                return _numbers.Count;
            }
        }

        public float? Mean
        {
            get
            {
                if (_numbers.Count == 0)
                {
                    return null;
                }

                float sum = 0;
                foreach (var num in _numbers)
                {
                    sum += num;
                }

                return sum / (float)_numbers.Count;
            }
        }

        public float? Min
        {
            get
            {
                if (_numbers.Count == 0)
                {
                    return null;
                }

                if (!_isSorted)
                {
                    _numbers.Sort();
                    _isSorted = true;
                }

                return _numbers[0];
            }
        }

        public float? Median
        {
            get
            {
                if (_numbers.Count == 0)
                {
                    return null;
                }

                if (!_isSorted)
                {
                    _numbers.Sort();
                    _isSorted = true;
                }

                return _numbers[(_numbers.Count - 1) / 2];
            }
        }

        public float? Max
        {
            get
            {
                if (_numbers.Count == 0)
                {
                    return null;
                }

                if (!_isSorted)
                {
                    _numbers.Sort();
                    _isSorted = true;
                }

                return _numbers[_numbers.Count - 1];
            }
        }

        public override string ToString()
        {
            if (_numbers.Count == 0)
            {
                return "No data";
            }

            return string.Format("Count {0} Mean {1} Min {2} Median {3} Max {4}", Count, Mean, Min, Median, Max);
        }
    }
}
