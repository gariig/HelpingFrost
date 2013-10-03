using System;
using System.Collections.Generic;
using System.Linq;
public class Supplier
{
    public string SupplierTradingNames { get; set; }
    public int TaxpayerID { get; set; }
    public string SupplierName { get; set; }
}

public class SupplierTradingName
{
    public int TaxpayerID { get; set; }
    public string SupplierTradingName1 { get; set; }
}

public class SupplierItemEntities
{
    public IEnumerable<Supplier> Suppliers
    {
        get
        {
            return new List<Supplier>();
        }
    }

    public IEnumerable<SupplierTradingName> SupplierTradingNames
    {
        get { return new List<SupplierTradingName>(); }
    }
}
public partial class NameModel
{
    private SupplierItemEntities db = new SupplierItemEntities();

    //Attempt to make a blank method to return object from sub methods
    // This gets called from the controllers
    /* 
     * I don't understand what you are trying to do here but removed because it's MVC stuff
    public static NameModel GetName(HttpContextBase context)
    {
        var tradename = new NameModel();
        return tradename;
    }
    
    //Attempt helper class to allow calls to extension methods
    // This is the method that calls the extension methods and pass them to tradename
    public static NameModel GetName(Controller controller)
    {
        return GetName(controller.HttpContext);
    }
    */

    public List<Supplier> MethodOne()
    {
        return db.Suppliers.ToList();
    }

    public IEnumerable<Supplier> MethodTwo()
    {
        var query = db.Suppliers.SelectMany(s => s.SupplierTradingNames, (s, t) => new { s.TaxpayerID, t });

        foreach (var group in query)
        {
            Console.WriteLine(group.TaxpayerID);

            foreach (var trade in group)
            {
                Console.WriteLine(" {0}", trade);
            }
        }
    }

    public IEnumerable<Supplier> MethodThree()
    {
        var query = from s in db.Suppliers
                    from t in db.SupplierTradingNames
                    where s.TaxpayerID == t.TaxpayerID
                    select new
                    {
                        Name = s.TaxpayerID,
                        tax = s.TaxpayerID,
                        trade = t.SupplierTradingName1
                    };
        return query.ToArray();
    }

    public List<Supplier> MethodFour()
    {
        var query = from s in db.Suppliers
                    join t in db.SupplierTradingNames
                    on s.TaxpayerID equals t.TaxpayerID
                    into g
                    from grp2 in g.DefaultIfEmpty()
                    select new
                    {
                        Tax = s.TaxpayerID,
                        Name = s.SupplierName,
                        Trade = grp2 != null ? grp2.SupplierTradingName1 : null
                    };
        return query.ToArray();
    }
    public IEnumerable<Supplier> MethodFive()
    {
        var query = db.Suppliers.ToLookup(s => s.SupplierTradingNames);
        foreach (var lookup in query)
        {
            Console.WriteLine(lookup.Key);
            foreach (var element in lookup)
            {
                Console.WriteLine(" {0}", element);
            }
        }
    }



    public List<Supplier> MethodSix()
    {
        var query = db.Suppliers.Join(db.SupplierTradingNames, s => s.TaxpayerID,
            t => t.SupplierTradingName1,
            (one, two) => new
            {
                Tax = one.TaxpayerID,
                EntiyName = one.SupplierName,
                TradingNames = two
            });

        foreach (var group in query)
        {
            Console.Write("{0}, {1}, {2}", group.Tax, group.EntiyName, group.TradingNames);

        }
    }

}
