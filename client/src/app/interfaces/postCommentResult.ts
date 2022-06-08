import { PostComment } from "./postComment";

export interface PostCommentResult {
    count: number;
    comments: PostComment[]
}