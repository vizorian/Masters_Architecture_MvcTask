// @ts-ignore
import {Genre} from "../Enums/Genre.tsx";

export interface Book {
    isbn13: string;
    isbn: string;
    title: string;
    author: string;
    description: string;
    yearPublished: number;
    genre: Genre;
}