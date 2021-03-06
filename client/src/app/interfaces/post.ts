import { PostComment } from "./postComment";

export interface Post {
    id: number;
    title: string;
    content: string;
    createdAt: Date;
    createdBy: string;
    comments: PostComment[];
    numberOfComments: number;
}