                                                    Information System for Fitness Centers
                                                         
This project is the implementation of a web application for an information system that allows for the management of fitness centers. The application uses three user roles: Visitor, Trainer, and Owner. The application handles the following entities:

        -User (Owner, Trainer, Visitor)
        -Training
        -Fitness Center

FUNCTIONALITIES:

    Unauthenticated User (Visitor):

        -On the homepage, the user can view all fitness centers (sorted by name in ascending order) that exist in the system in the form of a table.
        -The user can search for fitness centers by name, address, and year of opening (for the year of opening, allow defining a minimum and maximum limit for search          by year of opening).
        -Registration: Users can register on the application by filling in mandatory fields (username, password, first name, last name, gender, email, date of birth)            and optional fields (role, list of group training user is enrolled in, list of group trainings where the user is employed as a trainer, fitness center where            the user is employed, fitness centers owned by the user). Users can choose between roles: Visitor, Trainer, and Owner.

    Visitor:

        -Can sign up for a group trainings at a chosen fitness center.
        -Can view all information about a fitness center, including detailed center view, upcoming group trainings, and reviews from other visitors.

    Trainer:

        -Can view all information about a fitness center, including detailed center view, upcoming group trainings, and reviews from other visitors.
        -Can sign up for group trainings as a participant.
        -Can view and manage group trainings where they are employed as a trainer.
        -Can view and manage their profile and personal information.

    Owner:

        -Can view all information about their fitness centers, including detailed center view, upcoming group trainings, and reviews from other visitors.
        -Can add and manage group trainings for their fitness centers.
        -Can view and manage their profile and personal information.
    
    Group Trainings:

        -Users can browse and view all upcoming group trainings at various fitness centers.
        -Users can sign up for group trainings and receive confirmation.
        -Users can view their enrolled group trainings and cancel if needed.
        -Trainers and Owners can manage group trainings they are associated with.
    
    Fitness Center Details:

        -Users can view detailed information about fitness centers, including location, facilities and upcoming group trainings.
      
    Profile Management:

        -Users can view and manage their personal profile information, including username, password, email, and other optional fields.
        -Trainers and Owners can manage their associated fitness centers and group trainings.
    
    Search and Filters:

        -Users can search for fitness centers based on location, facilities, and other criteria.
        -Users can apply filters to narrow down their search results based on their preferences.
