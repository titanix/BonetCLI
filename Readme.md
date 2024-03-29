# Bonet CLI

Bonet CLI is a command line program to search nôm character, input Vietnamese diacritics and manipulate strings.

Run with `dotnet run`.

## Data Files

The `data` folders contains three files, each having a particular license.

|File|Licence|Source|
|---|---|---|
|wiki.txt|[CC BY-SA 3.0 Unported](https://creativecommons.org/licenses/by-sa/3.0/)|[Wiktionary Sino-Vietnamese Words List](https://en.wiktionary.org/w/index.php?title=Category:Sino-Vietnamese_words)|
|ids.xml|[GNU GPL v2 or later](https://gitlab.chise.org/CHISE/ids/-/blob/master/README.en)|[cjkvi-ids project]()|
|bonet.txt|[CC BY 4.0 International](https://creativecommons.org/licenses/by/4.0/)|[Bonet XXI project](https://github.com/titanix/Bonet_XXI)|

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

#### s ids_sequence

\> s ⿰氵國

```
Results list:
1. 漍
```

#### s stack_reference

```
Stack content:
1. vua
```

\> s 1

```
Results list:
1. 皮 vừa
2. 𤤰 vua
```

### [s]earch [n]et

Available only on Windows.

Make a web request to search a given string or stack reference. Use `h sn` to list the supported providers.

#### sn provider string
#### sn provider stack_reference

\> sn c lai 

### [d]ecompose [c]character

Decompose a character into its immediate constituents.

#### dc sinogram_1 ... sinogram_N

\> dc 吧

```
Results list:
1. 口
2. 巴
```

#### dc stack_reference_1 ... stack_reference_N

```
Stack content:
1. 𠀧
```

\> dc 1

```
Results list:
1. 三
2. 巴
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

### [v]iew [r]results

Display the content of the result list.

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

### [d]elete [f]rom

Delete items from the stack from a given index upwards.

```
Stack content:
1. a
2. b
3. c
4. d
```

\> df 3

```
Stack content:
1. a
2. b
```

### [d]elete [b]elow

Delete items from the stack below a given index.

```
Stack content:
1. a
2. b
3. c
```

\> db 2

```
Stack content:
1. b
2. c
```

### [n]ormalize [d]ecomposed

Normalize an input string to Unicode NFD (output to result list).

### [n]ormalize [c]omposed

Normalize an input string to Unicode NFC (output to result list).

### [m]acro [l]ist

List the macros usable in the current session.

\> ml

```
Macro list
1. => mm 2 1 ; a 3 ; d 3
2. => mm 2 4 1 3 ; a 5 ; d 3 4 5
3. => mm 4 2 3 1 ; a 5 ; d 3 4 5
4. => mm 2 3 1 4 ; a 5 ; d 3 4 5
```

### [m]acro [e]xecute

Execute a macro designated by its index in the macro list.

### [q]uit

Quit the application.

### [s]ave [s]tack

Save content of the stack into a file.

\> ss stack_save.txt

### [h]elp

Display help.

#### h command_name

Display help specific to a command.

\> h sn

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