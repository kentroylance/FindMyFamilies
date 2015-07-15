using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FindMyFamilies.Data;

namespace FindMyFamiles.Services.Util
{
    public class ListItemSort : IComparer
{
  /// <summary>
  /// IPersonNameSort Compare method
  /// </summary>
  int IComparer.Compare(object x, object y)
  {
     var source = (ListItemDO)x;
     var target = (ListItemDO)y;
    int result = string.Compare(source.DisplayMember, target.DisplayMember);
    return result;
  }
}
}


