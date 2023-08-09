namespace Orders.UnitTest.Domain.SeedWork;

public class Test_ValueObject
{

    [Theory]
    [MemberData(nameof(EqualityValueObjects))]
    public void Equals_EqualValueObjects_RetunsTrue(ValueObject instanceA, ValueObject instanceB, string reason)
    {
        // Action
        var result = EqualityComparer<ValueObject>.Default.Equals(instanceA, instanceB);

        // Assert
        Assert.True(result, reason);
    }

    [Theory]
    [MemberData(nameof(NotEqualityValueObjects))]
    public void Equals_NonEqualsValueObjects_ReturnsFalse(ValueObject instanceA, ValueObject instanceB, string reason)
    {
        // Action
        var result = EqualityComparer<ValueObject>.Default.Equals(instanceA,instanceB); 
        
        // Assert
        Assert.False(result, reason);
    }

    private static readonly ValueObject FakeValueObjectA = new ValueObjectA(1,"2",Guid.Parse("e1d9f840-5b2b-4392-b8cf-a3951258531c"),new ComplexObject(2,"3"));

    public static readonly TheoryData<ValueObject, ValueObject, string> EqualityValueObjects = new ()
    {
        {
            null, null, "they should be equals because they are boyh null"
        },
        {
            FakeValueObjectA,FakeValueObjectA,"they should be equals because they are the same objects"
        },
        {
            new ValueObjectA(5,"23",Guid.Parse("e1d9f840-5b2b-4392-b8cf-a3951258531c"), new ComplexObject(6,"24")),
            new ValueObjectA(5,"23",Guid.Parse("e1d9f840-5b2b-4392-b8cf-a3951258531c"), new ComplexObject(6,"24")),
            "they should be equals because they have equal members"
        },
        {
            new ValueObjectA(5,"23",Guid.Parse("e1d9f840-5b2b-4392-b8cf-a3951258531c"), new ComplexObject(6,"24"),"xpto"),
            new ValueObjectA(5,"23",Guid.Parse("e1d9f840-5b2b-4392-b8cf-a3951258531c"), new ComplexObject(6,"24"),"xpto 2"),
            "they should be equals because all equality components are equal, event though an additional member was set"
        },
        {
            new ValueObjectB(a: 1, b: "2", 1,2,3,4),
            new ValueObjectB(a: 1, b: "2", 1,2,3,4),
            "they should be equals because all equality components are equal, including the C list"
        }
    };

    public static readonly TheoryData<ValueObject, ValueObject, string> NotEqualityValueObjects = new ()
    {
        {
            new ValueObjectA(1,"23",Guid.Parse("e1d9f840-5b2b-4392-b8cf-a3951258531c"), new ComplexObject(6,"24")),
            new ValueObjectA(2,"23",Guid.Parse("e1d9f840-5b2b-4392-b8cf-a3951258531c"), new ComplexObject(6,"24")),
            "they should not be equals because of A member on ValueObjectA is different among them"
        },
        {
            new ValueObjectA(1,"23",Guid.Parse("e1d9f840-5b2b-4392-b8cf-a3951258531c"), new ComplexObject(6,"24")),
            new ValueObjectA(1,null,Guid.Parse("e1d9f840-5b2b-4392-b8cf-a3951258531c"), new ComplexObject(6,"24")),
            "they should not be equals because of B member on ValueObjectA is different among them"
        },
        {
            new ValueObjectA(1,"23",Guid.Parse("e1d9f840-5b2b-4392-b8cf-a3951258531c"), new ComplexObject(6,"24")),
            new ValueObjectA(1,"23",Guid.Parse("ba2e2ee9-4c67-4631-b7b9-8beede49e1de"), new ComplexObject(6,"24")),
            "they should not be equals because of C member on ValueObjectA is different among them"
        },
        {
            new ValueObjectA(1,"23",Guid.Parse("e1d9f840-5b2b-4392-b8cf-a3951258531c"), new ComplexObject(6,"24")),
            new ValueObjectA(1,"23",Guid.Parse("e1d9f840-5b2b-4392-b8cf-a3951258531c"), new ComplexObject(5,"24")),
            "they should not be equals because of D member on ValueObjectA is different among them"
        },
        {
            new ValueObjectA(1,"23",Guid.Parse("e1d9f840-5b2b-4392-b8cf-a3951258531c"), new ComplexObject(6,"24")),
            new ValueObjectB(1, "23", 1,2,3,4),
            "they should not be equals because they are not a same type"
        },
        {
            new ValueObjectB(1, "23", 1,2,3),
            new ValueObjectB(1, "23", 1,2,3,4),
            "they should be not equal because the list C contains additional value"
        },
        {
            new ValueObjectB(1, "23", 1,2,3, 5),
            new ValueObjectB(1, "23", 1,2,3),
            "they should be not equal because the list C contains additional value"
        },
        {
            new ValueObjectB(1, "23", 1,2,5),
            new ValueObjectB(1, "23", 1,2,3),
            "they should be not equal because C lists are not equals"
        }
    };

    private class ValueObjectA : ValueObject
    {
        public int A { get; set; }
        public string B { get; set; }
        public Guid C { get; set; }
        public ComplexObject D { get; set; }
        public string NotEqualComponent { get; set; }

        public ValueObjectA(int a, string b, Guid c, ComplexObject d, string notEqualComponent = null)
        {
            A = a; 
            B = b;
            C = c; 
            D = d; 
            NotEqualComponent = notEqualComponent;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return A;
            yield return B;
            yield return C;
            yield return D;
        }
    }

    private class ValueObjectB : ValueObject
    {
        public int A { get; }
        public string B { get; }
        public List<int> C { get; }

        public ValueObjectB(int a, string b, params int[] c)
        {
            A = a;
            B = b;
            C = c.ToList();
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return A;
            yield return B;

            foreach (var c in C)
            {
                yield return c;
            }
        }
    }

    private class ComplexObject : IEquatable<ComplexObject>
    {
        public int A { get; set; }
        public string B { get; set; }

        public ComplexObject(int a, string b)
        {
            A = a; 
            B = b;    
        }

        public override bool Equals(object obj)
            => Equals(obj as ComplexObject);

        public bool Equals(ComplexObject other)
            => other != null &&
                A == other.A &&
                B == other.B;

        public override int GetHashCode()
            => HashCode.Combine(A, B);
    }
}

