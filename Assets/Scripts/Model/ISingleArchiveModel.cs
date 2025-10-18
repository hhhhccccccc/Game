using System.Collections;
using System.Collections.Generic;

public interface ISingleArchiveModel : ISingleModel, IModel
{
    void Init(); 
    void Save();
}
