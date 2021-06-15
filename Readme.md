# Bonet IDE

## Commands

### [s]earch

Search for entries using quoc ngu without diacritics or by composition of Chinese character components. Space in-between arguments can be either ASCII or Chinese space.

#### s quoc_ngu_without_diacritics

\> s boi

```
Results list:
1. 輩 bối
2. 賠 bồi
3. 背 bội
4. 徘 bồi
5. 背 bối
6. 培 bồi
```

#### s sinogram_1 ... sinogram_N

\> s 不 皿

```
Results list:
1. 益
2. 𢙂
3. 𤄼
4. 𥁞
5. 𥂭
6. 𥃆
7. 𥃉
8. 𦡿
9. 𦾗
10. 𧨦
11. 𭾊
12. 盃
```

### [p]ush on stack

Push a result from a results list into the stack and display the stack content.

#### p result_list_reference

\> s o

```
Results list:
1. 惡 ố
2. 污 ô
3. 烏 ô
```

\> p 1
```
Stack content:
1. 惡 ố
```

#### p string_literal

\> p A
```
Stack content:
1. A
```

Note: spaces are ignored and the whole line except command name is treated as the literal to add to the stack.

### [m]erge stack entries

Add a new entry on the stack that is the concatenation of stack references or literals.

#### m stack_reference_1|string_literal_1 ... stack_reference_N|string_literal_1

```
Stack content:
1. a
2. b
3. c
```

\> m 1 2 3
```
Stack content:
1. a
2. b
3. c
4. abc
```

\> m 1 _ 2
```
Stack content:
1. a
2. b
3. c
4. abc
5. a_b
```

### [m]erge with [s]pace

Merge stack entries or literal by concatenation and adding a space between the elements.

```
Stack content:
1. a
2. b
```

\> ms a _ b

```
Stack content:
1. a
2. b
3. a _ b
```

### [a]dd to dictionary

Add a literal to the dictionary under edition.

\> a 烏 ô

No feedback is given on the operation.

### [q]uit

Quit the application.

### [h]elp

Display help.