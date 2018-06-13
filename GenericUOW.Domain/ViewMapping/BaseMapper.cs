using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace GenericUOW.Domain.ViewMapping
{
    public class BaseMapper<Src, Dst>
    {
        public IMapper Mapper { get; set; }
        public List<Dst> MapToModelList(IEnumerable<Src> model)
        {
            List<Dst> mappedList = new List<Dst>();

            foreach (Src sourceModel in model)
                mappedList.Add(Mapper.Map<Src, Dst>(sourceModel));

            return mappedList;
        }

        public List<Src> MapToViewList(IEnumerable<Dst> view)
        {
            List<Src> mappedList = new List<Src>();

            foreach (Dst sourceView in view)
                mappedList.Add(Mapper.Map<Dst, Src>(sourceView));

            return mappedList;
        }

        public Src MapToView(Dst view)
        {
            return Mapper.Map<Dst, Src>(view);
        }

        public Dst MapToModel(Src view)
        {
            return Mapper.Map<Src, Dst>(view);
        }
    }
}
