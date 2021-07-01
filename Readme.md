# Bonet IDE

## Commands

### [s]earch

Search for entries using quốc ngữ without diacritics or by composition of Chinese character components. Space in-between arguments can be either ASCII or Chinese space.

In letter search, the sequence `dd` is replaced by `đ`.

Search command can use numbers for references on the stack.

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

### [d]ecompose [c]character

Decompose a character into its immediate constituents.

#### dc sinogram_1 ... sinogram_N

\> dc 吧

```
Results list:
1. 口
2. 巴
```

\> dc 蟹

```
Results list:
1. 解
2. 虫
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

\> p A B
```
Stack content:
1. A B
```

Note: spaces are ignored and the whole line except command name is treated as the literal to add to the stack.

#### Implicit push

If no command is used and a character superior or equal to U+2E80 is present on the input line, the content of the line is added to the stack.

\> 為

```
Stack content:
1. 為
```

### [p]ush [s]plit

Push multiple values on the stack either from a list result or literals. Content of the list result or the list of literals are split on spaces before adding resulting elements to the stack.

```
Results list:
1. 惡 ố
```

\> ps 1
```
Stack content:
1. 惡
2. ố
```

\> ps a b
```
Stack content:
1. a
2. b
```


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

### [m]erge [m]agic

Merge strings in quốc ngữ append space after each syllables and nôm without inserting space. Result is pushed to the stack. The resulting string is in the format expected by the dictionary and can be used with the [a]dd command.

```
Stack content:
1. bình
2. 平
```

\> mm 1 an 2 安

```
Stack content:
1. bình
2. 平
3. bình an 平安
```

### [a]dd to dictionary

Add a literal to the dictionary under edition.

\> a 烏 ô

No feedback is given on the operation.

### [c]onvert 

Convert a letter sequence into another and add the result to the stack. Used to add tone diacritic to syllable.

The convert command can also convert some CAPS letters to IDS symbols (use [hi] to see allowed conversions).

\> c aa

```
Stack content:
1. ă
```

\> c luân4
```
Stack content:
1. luận
```

### [v]iew [s]tack

Display the content of the stack.

### [c]lear [s]tack

Empty the stack.

```
Stack content:
1. a
2. b
```

\> cs

```
Stack content:
```

### [d]elete

Delete one or more items from the stack.

```
Stack content:
1. a
2. b
3. c
4. d
```

\> d 2 4

```
Stack content:
1. a
2. c
```

### [q]uit

Quit the application.

### [h]elp

Display help.

### [h]elp [t]ones

Get information about tone number and diacritics.

### [h]elp [i]ds

Get information the IDS conversion symbols.

## Chaining Commands

Multiple commands can be chained if separated by a semi-colon.

\> ms a 阿 ; a 1

```
Stack content:
1. a 阿
Content added.
```