#----------------------------------------------------
# Name: Joe Daniels
# Usage: Daily - Advent production
# Description: Script will generate an Advent price file from data files provided by Bloomberg via Eaton Vance.
#
# CA Command: C:\Scripts\_Infra\PortAcct\OPS_Pricing_FileCreate.pl %(DataFeed)\Advent\PRICE_FILE\ %( APPL.datestampMMDDYY)%( APPL.datestampMMDDYY)%( APPL.datestampMMDDYY).pri %( APPL.datestampMMDDYY)%( APPL.datestampMMDDYY)%( APPL.datestampMMDDYY)_new.pri stale_%( APPL.datestampMMDDYY)%( APPL.datestampMMDDYY)%( APPL.datestampMMDDYY).csv
# Last modified: 4/11/11
#----------------------------------------------------

#----------------------------------------------------
# File Paths
$indir = $ARGV[0]; ## \\paraport\resources\DataFeed_TEST\Advent\PRICE_FILE\
# File Names
$prifile = $ARGV[1]; ## MMDDYY.pri
$outfile = $ARGV[2]; ## MMDDYY_new.pri
$stalefile = $ARGV[3]; ## stale_MMDDYY.csv
$infile = $ARGV[4]; ## BB region file.px
$mapfile = "BB_mapping.txt";
#----------------------------------------------------

# Set the working directory.
if ( !(chdir $indir) ){
    print "Source path, $indir, does not exist. Exiting with failure.\n";
    exit 1;
}

# Open the Advent price file. Exit with error code 1 if unable to.
if ( !(open (PRIFILE, "<$prifile")) ){
    print "Unable to open Advent price file, $prifile. Exiting with failure.\n";
    exit 1;
}

# Read the $prifile into an array to get Advent price data
while ( <PRIFILE> ) {    
    chomp;
    push @priline,[split /,/];
}
close PRIFILE;

# Create the new Advent price file. Exit with error code 1 if unable to.
if ( !(open (OUTFILE,">$outfile")) ){
    print "Unable to create Advent new price file, $outfile. Exiting with failure.\n";
    exit 1;
}

# Create the mapping file for revised pricing process
if ( !(open (MAPFILE,">$mapfile")) ) {
		print "Unable to append to the Mapping file, $mapfile. Exiting with failure.\n";
		exit 1;
}

# Open the stale file. Exit with error code 1 if unable to.
if ( !(open (STALEFILE, "$stalefile")) ){
	print "Unable to open the stale price file, $stalefile. Exiting with failure.\n";
	exit 1;
}

# Read the $stalefile into an array to get Stale prices
while (my $line = <STALEFILE> ) {    
    chomp($line);
    push @staleline,[split /,/, $line];
}
close STALEFILE;

# Create a hash of the tickers in Advent price file - so we only store BB data that we'll need    
my %ticklist;
$count = 0;
foreach ( @priline ){
		$ticklist{$priline[$count][1]} = 1;
		$count++;
}

# Open the price files provided by Bloomberg. Exit with error code 1 if unable to.
if ( !(open (INFILE, "$infile")) ){
		print "Unable to open Bloomberg price file, $infile. Exiting with failure.\n";
    exit 1;
}

# Read the $infile into an array to get Bloomberg price data - cross-reference hash to filter data
while (my $line = <INFILE> ) {    
		chomp($line);
	  my @subline = split(/\|/, $line);
	  $subline[3] =~ s/\//\./;
    if (exists $ticklist{$subline[3]} || exists $ticklist{substr($subline[5],0,6)} || exists $ticklist{substr($subline[14],0,8)}) {
				push @inline,[split /\|/, $line];
		}
}
close INFILE;

# Add Bloomberg price data to Advent price file
$n = 0;
foreach ( @priline ) {
		$stale = 0;
		$stype = $priline[$n][0];
		$ticker = $priline[$n][1];
		$price = $priline[$n][2];
		$advcur = uc(substr($stype,2,2));
		$bbunique = "";

	# Skip if we've already priced this security
	if ( $price == '' ) {
		# Determine whether we should stale price this security
		$a = 0;
		STALE: foreach ( @staleline ) {
				if ( $ticker eq $staleline[$a][0] ) {
						$price = $staleline[$a][1];
						$stale = 1;
						last STALE;
				}
				$a++;
		}

		#Convert "." to "/" for BB search
		$ticker =~ s/\./\//;
		
		# Get the Bloomberg price for this ticker
		if ( $stale == 0 ) {
				$b = 0;
				BB: foreach ( @inline ) {
					  $bbtick = $inline[$b][3];
					  $bbsedol = substr($inline[$b][5],0,6);
					  $bbcusip = substr($inline[$b][14],0,8);
					  $bbcur = substr($inline[$b][34],0,2);
					  $bbprice = $inline[$b][26];
						$bbfactor = $inline[$b][36];
						$bbunique = "";
					  
						#Try the Bloomberg ticker & ensure proper currency ... WIP!!!
						if ( $ticker eq $bbtick && $advcur eq $bbcur ) {
								$price = $bbprice;
								$bbunique = $inline[$b][49];
								delete $inline[$b];
								last BB;
						}
						#Try the Bloomberg sedol & ensure proper currency
						if ( $ticker eq $bbsedol && $advcur eq $bbcur ) {
								$price = $bbprice;
								$bbunique = $inline[$b][49];
								delete $inline[$b];
								last BB;
					}
						#Try the Bloomberg cusip & ensure proper currency
						if ( $ticker eq $bbcusip && $advcur eq $bbcur ) {
								$price = $bbprice;
								$bbunique = $inline[$b][49];
								delete $inline[$b];
								last BB;
						}
						else {
								$price = "";
						}
						$b++;
				}
		#Checks for any value that will "weak type" to 0, and set it to 1 in that case.
		$bbfactor ||= 1;
		
		#Final validations and checks
		$price = $price / $bbfactor;
		print "$bbsedol\n";
		if ( $stype eq "csbr" ) {
				$price = $price * 1000;
		}
		if ( $stype eq "csgb" ) {
				$price = $price / 100;
		}
		if ( $stype eq "csil" ) {
				$price = $price / 100;
		}
		if ( $stype eq "csza" ) {
				$price = $price / 100;
		}
		if ( $stype eq "csbw" ) {
				$price = $price / 100;
		}
		if ( $stype eq "cskw" ) {
				$price = $price / 1000;
		}	
		if ( $price eq "N.A." ) {
				$price = "";
		}
		if ( $price eq 0 ) {
				$price = "";	
		}
	}
	}
	$ticker =~ s/\//\./;
	#Write price lines to the new Advent price file
	print OUTFILE "$stype,$ticker,$price,,2\n";
	#Write the ticker and Bloomberg id to the mapping file for updated price file process
	if ($bbunique ne "") {
		print MAPFILE "$ticker,$bbunique\n";
	}
	$n++;
}

# Close open file handlers
close OUTFILE;
close MAPFILE;

# Exit with success
exit 0;
