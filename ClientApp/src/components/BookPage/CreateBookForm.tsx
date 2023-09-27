import {Formik, Form, Field, ErrorMessage} from 'formik';
import * as Yup from 'yup';
// @ts-ignore
import {Genre} from "./Enums/Genre.tsx";
// @ts-ignore
import {Book} from "./Models/Book.tsx";

const ISBN13_LENGTH = 13;
const ISBN_LENGTH = 10;

const MINIMUM_PUBLISHED_YEAR = 1450;
const MAXIMUM_PUBLISHED_YEAR = new Date().getFullYear();

const ISBN13_REGEX = /^(978|979)\d{10}$/;
const ISBN10_REGEX = /^\d{9}[\dX]$/;

export const DEFAULT_BOOK_FORM_INPUT: Book = {
    isbn13: "",
    isbn: "",
    title: "",
    author: "",
    description: "",
    yearPublished: new Date().getFullYear(),
    genre: Genre.Other
}

const BookValidationSchema = Yup.object().shape({
    isbn13: Yup.string()
        .length(ISBN13_LENGTH, `Must be ${ISBN13_LENGTH} characters`)
        .matches(ISBN13_REGEX, 'Invalid ISBN13. Must start with 978 or 979 and be followed by 10 digits.')
        .when('isbn', {
            is: (isbn) => !isbn,
            then: schema => schema.required('Required'),
        }),
    isbn: Yup.string()
        .length(ISBN_LENGTH, `Must be ${ISBN_LENGTH} characters`)
        .matches(ISBN10_REGEX, 'Invalid ISBN10. Must be 9 digits followed by a digit or X.')
        .when('isbn13', {
            is: (isbn13) => !isbn13,
            then: schema => schema.required('Required'),
        }),
    title: Yup.string()
        .required('Required'),
    author: Yup.string()
        .required('Required'),
    description: Yup.string()
        .optional(),
    yearPublished: Yup.number()
        .required('Required')
        .min(MINIMUM_PUBLISHED_YEAR, `Must be between ${MINIMUM_PUBLISHED_YEAR} and ${MAXIMUM_PUBLISHED_YEAR}`)
        .max(MAXIMUM_PUBLISHED_YEAR, `Must be between ${MINIMUM_PUBLISHED_YEAR} and ${MAXIMUM_PUBLISHED_YEAR}`),
    genre: Yup.string()
        .required('Required')
}, [['isbn13', 'isbn']]);

interface Props {
    handleCancel: () => void;
    handleSubmit: (book: Book) => void;
}

export default function CreateBookForm(props: Props) {
    const genreList = Object
        .values(Genre)
        .filter((genre: string) => Number.isNaN(Number(genre)))
        .map((genre: string) => <option key={genre} value={genre}>{genre}</option>);

    return (
        <Formik
            initialValues={DEFAULT_BOOK_FORM_INPUT}
            validationSchema={BookValidationSchema}
            onSubmit={(values, {setSubmitting}) => {
                setTimeout(() => {
                    props.handleSubmit(values);
                    setSubmitting(false);
                }, 400);
            }}
        >
            {({isSubmitting}) => (
                <Form className={"form form--create"}>
                    <h3>ISBN13:</h3>
                    <Field type="text" name="isbn13"/>
                    <ErrorMessage name="isbn13" component="div"/>
                    <h3>ISBN10:</h3>
                    <Field type="text" name="isbn"/>
                    <ErrorMessage name="isbn" component="div"/>
                    <h3>Title</h3>
                    <Field type="text" name="title"/>
                    <ErrorMessage name="title" component="div"/>
                    <h3>Author</h3>
                    <Field type="text" name="author"/>
                    <ErrorMessage name="author" component="div"/>
                    <h3>Description</h3>
                    <Field type="text" name="description"/>
                    <ErrorMessage name="description" component="div"/>
                    <h3>Year Published</h3>
                    <Field type="number" name="yearPublished"/>
                    <ErrorMessage name="yearPublished" component="div"/>
                    <h3>Genre</h3>
                    <Field as="select" name="genre">
                        {genreList}
                    </Field>
                    <ErrorMessage name="genre" component="div"/>
                    <button type="submit" disabled={isSubmitting}>
                        Submit
                    </button>
                    <button type={"button"} onClick={props.handleCancel}>Cancel</button>
                </Form>
            )}
        </Formik>
    );
}