export interface PostComment {
    content: string;
    createdAt: Date;
    id: number;
    postId: number;
    username: string
}