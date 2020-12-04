using appWeb.Common.Entities;
using appWeb.Web.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace appWeb.Web.Helpers
{
    public class AIHelper : IAIHelper
    {
        private readonly DataContext _context;

        public AIHelper(DataContext context)
        {
            _context = context;
        }

        public async Task<List<History>> GetBestAndWorst()
        {
            List<string> categories = new List<string>
            {
                "Technology",
                "Sports",
                "Art"
            };
            List<History> list = new List<History>();
            Stack<History> stack = new Stack<History>();
            Stack<History> stack2 = new Stack<History>();
            stack.Push(new History { Prediction = 0.0 });
            stack2.Push(new History { Prediction=10000000.0 });
            foreach (string item in categories)
            {
                var ax = await _context.Histories
                    .FirstOrDefaultAsync(h => h.Name == item);
                var aux = await _context.Histories
                    .Where(h => h.Name == item ).ToListAsync();
                double alfa = 2/(aux.Count+1);
                int sum = 0;
                foreach (var j in aux)
                {
                    sum += j.Number;
                }
                double se = SuavizadoExp(alfa,sum/aux.Count,ax.Prediction,sum);
                History x = stack.Peek();
                History x2 = stack2.Peek();
                if (se > x.Prediction)
                {
                    stack.Pop();
                    ax.Prediction = se;
                    stack.Push(ax);
                }
                else if (se < x2.Prediction)
                {
                    stack2.Pop();
                    ax.Prediction = se;
                    stack2.Push(ax);
                }
                list.Add( new History { Name=ax.Name,Number=ax.Number,Prediction=se });
                _context.Histories.RemoveRange(aux);
            }
            await ReFill(list);
            return new List<History>
            {
                stack.Pop(),
                stack2.Pop()
            };
        }

        private async Task ReFill(List<History> list)
        {
            _context.AddRange(list);
            await _context.SaveChangesAsync();
        } 

        private double SuavizadoExp(double alfa, double x, double xi, int inv)
        {
            return xi + (alfa*(xi-inv));
        }
    }
}
