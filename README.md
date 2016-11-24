# lp-solver
simplex-algorithm

The algebraic classes are programmed for the simplex-algorithm. They are not tested for the use of other scenarios.
## usage
`new Solver()` requires an Array of `Equations` or a filename respectively `Stream` of a declaration in csv like this:
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

