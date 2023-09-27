import {Formik, Form, Field, ErrorMessage} from 'formik';
import * as Yup from 'yup';
// @ts-ignore
import {Genre} from "./Enums/Genre.tsx";
// @ts-ignore
import {BookDto} from "./Models/BookDto.tsx";

const MINIMUM_PUBLISHED_YEAR = 1450;
const MAXIMUM_PUBLISHED_YEAR = new Date().getFullYear();

interface Props {
    handleCancel: () => void;
    handleSubmit: (book: BookDto) => void;
    book: BookDto;
}

const BookValidationSchema = Yup.object().shape({
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
});

export default function UpdateBookForm(props: Props) {
    const genreList = Object
        .values(Genre)
        .filter((genre: string) => Number.isNaN(Number(genre)))
        .map((genre: string) => <option key={genre} value={genre}>{genre}</option>);

    return (
        <Formik
            initialValues={props.book}
            validationSchema={BookValidationSchema}
            onSubmit={(values, {setSubmitting}) => {
                setTimeout(() => {
                    props.handleSubmit(values);
                    setSubmitting(false);
                }, 400);
            }}
        >
            {({isSubmitting}) => (
                <Form className={"form form--update"}>
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