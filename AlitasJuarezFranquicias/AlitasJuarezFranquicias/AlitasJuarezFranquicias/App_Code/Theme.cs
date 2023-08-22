using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Theme
{
    private string _nombre;

    public string Nombre
    {
        get { return _nombre; }
        set { _nombre = value; }
    }

	public Theme(string nombre)
	{
        Nombre = nombre;
	}
}