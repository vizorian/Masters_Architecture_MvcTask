// @ts-ignore
import {Genre} from "../Enums/Genre.tsx";

export interface BookDto {
    title: string;
    author: string;
    description: string;
    yearPublished: number;
    genre: Genre;
}