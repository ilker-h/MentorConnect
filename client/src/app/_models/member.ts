import { Photo } from "./photo";

export interface Member {
  id: number;
  userName: string;
  photoUrl: string;
  age: number;
  yearsOfCareerExperience: number;
  knownAs: string;
  created: string;
  lastActive: string;
  mentorOrMentee: string;
  bio: string;
  lookingFor: string;
  careerInterests: string;
  city: string;
  country: string;
  photos: Photo[];
}

