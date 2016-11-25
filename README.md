# lp-solver (using Simplex Algorithm)

There are different solutions to setup the Simplex Table and solved it. If you dig into the code behind this Solver don't be confused if it is different from other Examples in the Web and at University

The algebraic classes are programmed for this simplex-algorithm. They are very limited and not tested for the use of other scenarios.
## usage
`new Solver()` requires an Array of `Equations` or a filename respectively a `Stream` of a declaration in csv like this:
```
2;3;;
max;3;2;0
 true; true;;
<=;2;1;100
<=;1;1;80
<=;1;0;40

```

## relevant Types
### Solver class
### Equation struct
### Variable & VariableFactor structs
### Tableau struct
`Tableau` represents a collection of `Equation`s. `Tableau` is immutable. 

`public bool FindPivot(out Variable head, out Variable row)` returns true if a valid PivotElement exists on the `Tableau`. The `head`and `row` values will indicate the concerned row and column.

`public static Tableau NextTableau(Tableau t, Variable pHead, Variable pRow)` returns a new Tableau by applying Simplex Iteration with the PivotElement given by `pHead` and `pRow`.